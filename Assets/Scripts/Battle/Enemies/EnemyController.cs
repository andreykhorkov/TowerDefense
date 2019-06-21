using System;
using System.Collections;
using Pool;
using UnityEngine;
using UnityEngine.AI;
using VFX;
using Zenject;

public class EnemyArgs : EventArgs
{
    public int Id { get; private set; }
    public EnemyType EnemyType { get; private set; }

    public EnemyArgs(int id, EnemyType enemyType)
    {
        Id = id;
        EnemyType = enemyType;
    }
}

public enum EnemyType
{
    simple,
    advanced
}

// a bit overkill for now since both nested enemy classes are pretty much the same
public abstract class EnemyController : PoolObject
{
    [SerializeField] protected EnemyParams enemyParams;
    [SerializeField, AssetPathGetter] private string explosionAssetPath;
    [Inject] private BattleController battleController;
    [Inject] private EnemySpawnController enemySpawnController;
    [Inject] private ParticleEffect.Factory factory;
    
    protected NavMeshAgent navMeshAgent;

    protected Collider collider;

    protected int health;

    public abstract string VehicleName { get; }

    public abstract EnemyType EnemyType { get; }

    public int Id { get; private set; }

    public float Speed { get { return navMeshAgent.speed; } }

    public static event EventHandler<EnemyArgs> EnemyInstantiated = delegate { };

    public static event EventHandler<EnemyArgs> EnemyDestroyed = delegate { };

    private void Awake()
    {
        Initialize();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.FinishLayer)
        {
            ReturnObject();
        }
    }

    private void OnProjectileHitCollider(object sender, HitEnemyColArgs enemyColArgs)
    {
        if (collider != enemyColArgs.Collider)
        {
            return;
        }

        var projectile = sender as Projectile;
        var damage = projectile.ProjectileParams.Damage;
        TakeDamage(damage);
    }

    private void OnTakesDamage(object sender, HitEnemyArgs hitEnemyArgs)
    {
        if (Id != hitEnemyArgs.Id)
        {
            return;
        }

        TakeDamage(hitEnemyArgs.Damage);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath, factory);
            explosion.transform.position = transform.position;
            explosion.Play();

            ReturnObject();
            EnemyDestroyed(this, new EnemyArgs(Id, EnemyType));
            battleController.BattleStats.CountFrag(EnemyType);
        }
    }

    protected void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Id = BattleController.LastEnemyId + 1;
        name = $"{VehicleName}_{Id}";
        collider = GetComponent<Collider>();
  
        Projectile.HitEnemyColliderHandler += OnProjectileHitCollider;
        Projectile.HitEnemyHandler += OnTakesDamage; 
        EnemyInstantiated(this, new EnemyArgs(Id, EnemyType));
    }

    public void ForceHitByTheProjectile(Projectile projectile)
    {
        projectile.ReturnObject();
        OnProjectileHitCollider(this, new HitEnemyColArgs(projectile.GetComponent<Collider>()));
    }

    public override void OnTakenFromPool()
    {
        health = enemyParams.DefaultHealth;
        gameObject.layer = BattleController.EnemyLayer;
    }

    public void SetGoalDestination()
    {
        navMeshAgent.Warp(enemySpawnController.GetSpawnPos());
        navMeshAgent.enabled = true;
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination(BattleController.GoalPosition);
    }

    public void AddSpeed(float addedSpeed)
    {
        navMeshAgent.speed = enemyParams.DefaultSpeed + addedSpeed;
    }

    public override void OnReturnedToPool()
    {
        HelpTools.ChangeLayersRecursively(transform, BattleController.PoolLayer);
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
        navMeshAgent.enabled = false;
        EnemyDestroyed(this, new EnemyArgs(Id, EnemyType));
    }

    public override void OnPreWarmed()
    {
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
        navMeshAgent.enabled = false;
        gameObject.layer = BattleController.PoolLayer;
    }

    void OnDestroy()
    {
        Projectile.HitEnemyColliderHandler -= OnProjectileHitCollider;
    }

    public class Factory : PlaceholderFactory<string, EnemyController> { }
}

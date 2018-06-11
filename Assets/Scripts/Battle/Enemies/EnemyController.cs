using System;
using System.Collections;
using Pool;
using UnityEngine;
using UnityEngine.AI;
using VFX;

public class EnemyArgs : EventArgs
{
    public int Id { get; private set; }

    public EnemyArgs(int id)
    {
        Id = id;
    }
}

public abstract class EnemyController : PooledBattleElement
{
    [SerializeField] protected EnemyParams enemyParams;
    [SerializeField] protected ProjectileParams projectileParams;
    [SerializeField, AssetPathGetter] private string explosionAssetPath;
    
    protected NavMeshAgent navMeshAgent;

    protected Collider collider;

    protected int health;

    public abstract string VehicleName { get; }

    public int Id { get; private set; }

    public float Speed { get { return navMeshAgent.speed; } }

    public static event EventHandler<EnemyArgs> EnemyInstantiated = delegate { };

    public static event EventHandler<EnemyArgs> EnemyDestroyed = delegate { };

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.LevelBoundsLayer)
        {
            ReturnObject();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.ProjectileLayer)
        {
            var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath);
            explosion.SetOrientation(collider.transform.position, Quaternion.identity);
            explosion.Play();
        }
    }

    private void OnTakesDamage(object sender, HitArgs args)
    {
        if (collider != args.Collider)
        {
            return;
        }

        var projectile = sender as Projectile;
        health -= projectile.ProjectileParams.Damage;

        if (health <= 0)
        {
            ReturnObject();
            EnemyDestroyed(this, new EnemyArgs(Id));
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            ReturnObject();
            EnemyDestroyed(this, new EnemyArgs(Id));
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        navMeshAgent = GetComponent<NavMeshAgent>();
        Id = BattleController.LastEnemyId + 1;
        name = string.Format("{0}_{1}", VehicleName, Id);
        collider = GetComponent<Collider>();
        PoolManager.PreWarm<ParticleEffect>(explosionAssetPath, 5);
        Projectile.HitEnemyHandler += OnTakesDamage;

        EnemyInstantiated(this, new EnemyArgs(Id));
    }

    public override void OnTakenFromPool()
    {
        health = enemyParams.DefaultHealth;
    }

    public IEnumerator SetGoalDestination()
    {
        navMeshAgent.enabled = true;
        yield return null;
        yield return null;
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination(BattleController.GoalPosition);
    }

    public void AddSpeed(float addedSpeed)
    {
        navMeshAgent.speed = enemyParams.DefaultSpeed + addedSpeed;
    }

    public override void OnReturnedToPool()
    {
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
        navMeshAgent.enabled = false;
        EnemyDestroyed(this, new EnemyArgs(Id));
    }

    public override void OnPreWarmed()
    {
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
        navMeshAgent.enabled = false;
    }

    public abstract void Move();

    public virtual void Update()
    {
    }

    void OnDestroy()
    {
        Projectile.HitEnemyHandler -= OnTakesDamage;
    }
}

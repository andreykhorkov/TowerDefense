using System;
using Pool;
using UnityEngine;
using UnityEngine.AI;
using VFX;

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
public abstract class EnemyController : PooledBattleElement
{
    [SerializeField] protected EnemyParams enemyParams;
    [SerializeField] protected ProjectileParams projectileParams;
    [SerializeField, AssetPathGetter] private string explosionAssetPath;
    
    protected NavMeshAgent navMeshAgent;

    protected Collider collider;

    protected int health;

    public abstract string VehicleName { get; }

    public abstract EnemyType EnemyType { get; }

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
            var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath);
            explosion.SetOrientation(transform.position, Quaternion.identity);
            explosion.Play();

            ReturnObject();
            EnemyDestroyed(this, new EnemyArgs(Id, EnemyType));
            BattleRoot.BattleController.BattleStats.CountFrag(EnemyType);
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

        EnemyInstantiated(this, new EnemyArgs(Id, EnemyType));
    }

    public override void OnTakenFromPool()
    {
        health = enemyParams.DefaultHealth;
        gameObject.layer = BattleController.EnemyLayer;
    }

    public void SetGoalDestination()
    {
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
        Projectile.HitEnemyHandler -= OnTakesDamage;
    }
}

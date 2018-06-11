using System;
using System.Collections;
using Pool;
using Projectiles;
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
    [SerializeField, AssetPathGetter] private string explosionAssetPath;
    
    protected NavMeshAgent navMeshAgent;

    public abstract string VehicleName { get; }

    public int Id { get; private set; }

    public float Speed { get { return navMeshAgent.speed; } }

    public static event EventHandler<EnemyArgs> EnemyInstantiated = delegate { }; 

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

    protected override void Initialize()
    {
        base.Initialize();

        navMeshAgent = GetComponent<NavMeshAgent>();
        Id = BattleController.LastEnemyId + 1;
        name = string.Format("{0}_{1}", VehicleName, Id);
        PoolManager.PreWarm<ParticleEffect>(explosionAssetPath, 5);

        EnemyInstantiated(this, new EnemyArgs(Id));
    }

    public override void OnTakenFromPool()
    {
        
    }

    public IEnumerator SetGoalDestination()
    {
        navMeshAgent.enabled = true;
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
}

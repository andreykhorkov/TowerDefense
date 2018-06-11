using System;
using System.Collections;
using Enemies;
using Pool;
using UnityEngine;
using UnityEngine.AI;

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

    protected EnemyState idleState;
    protected EnemyState normalState;
    protected EnemyState currentState;
    protected int levelBoundsLayer;
    protected NavMeshAgent navMeshAgent;
    protected BattleController BattleController;
    protected Vector3 pooledPosition;

    public abstract string VehicleName { get; }

    public int Id { get; private set; }

    public static event EventHandler<EnemyArgs> EnemyInstantiated = delegate { };

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == levelBoundsLayer)
        {
            ReturnObject();
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        BattleController = BattleRoot.BattleController;
        pooledPosition = PoolManager.Instance.transform.position;

        idleState = new IdleState(this);
        normalState = new NormalState(this);
        currentState = normalState;

        levelBoundsLayer = LayerMask.NameToLayer("LevelBounds");

        navMeshAgent = GetComponent<NavMeshAgent>();
        Id = BattleController.LastEnemyId + 1;
        name = string.Format("{0}_{1}", VehicleName, Id);

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
        SetOrientation(pooledPosition, Quaternion.identity);
        navMeshAgent.enabled = false;
    }

    public override void OnPreWarmed()
    {
    }

    public abstract void Move();

    public virtual void Update()
    {
    }
}

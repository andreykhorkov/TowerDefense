using System;
using System.Collections;
using Enemies;
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
    protected BattleController battleController;

    public abstract string VehicleName { get; }

    public int Id { get; private set; }

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

        battleController = BattleRoot.BattleController;

        idleState = new IdleState(this);
        normalState = new NormalState(this);
        currentState = normalState;

        levelBoundsLayer = LayerMask.NameToLayer("LevelBounds");

        navMeshAgent = GetComponent<NavMeshAgent>();
        Id = BattleController.LastEnemyId + 1;
        name = string.Format("{0}_{1}", VehicleName, Id);
        Debug.Log(name);

        battleController.SetLastEnemyId(Id);
    }

    public override void OnTakenFromPool()
    {
        
    }

    public IEnumerator SetGoalDestination()
    {
        navMeshAgent.enabled = true;
        yield return null;
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination(battleController.GoalPosition);
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
        Debug.LogError(name);
    }

    public abstract void Move();

    public virtual void Update()
    {
    }
}

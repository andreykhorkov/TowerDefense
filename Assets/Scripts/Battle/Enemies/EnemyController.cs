using Enemies;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : PooledBattleElement
{
    protected EnemyState idleState;
    protected EnemyState normalState;
    protected EnemyState currentState;
    protected int levelBoundsLayer;
    protected NavMeshAgent navMeshAgent;

    public Transform target;

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == levelBoundsLayer)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        idleState = new IdleState(this);
        normalState = new NormalState(this);
        currentState = normalState;

        levelBoundsLayer = LayerMask.NameToLayer("LevelBounds");

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(target.position);
    }

    public abstract void Move();

    public virtual void Update()
    {
    }
}

using Projectiles;
using UnityEngine;

public class Projectile : PooledBattleElement
{
    private SphereCollider sphereCollider;
    private TrailRenderer trailRenderer;
    private IdleState idleState;
    private ThrownState thrownState;
    private ProjectileState currentState;
    private Vector3 throwDirection;

    [SerializeField] private ProjectileParams projectileParams;

    protected override void Initialize()
    {
        base.Initialize();

        sphereCollider = GetComponent<SphereCollider>();
        trailRenderer = GetComponent<TrailRenderer>();

        idleState = new IdleState(this);
        thrownState = new ThrownState(this);
    }

    public override void OnReturnedToPool()
    {
        sphereCollider.enabled = false;
        trailRenderer.enabled = false;
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
        SetState(idleState);
    }

    public void SetState(ProjectileState state)
    {
        if (currentState != null)
        {
            currentState.OnStateLeave();
        }

        currentState = state;
        currentState.OnStateEntered();
    }

    public override void OnTakenFromPool()
    {
        sphereCollider.enabled = true;
        trailRenderer.enabled = true;
    }

    public void Throw(Vector3 dir)
    {
        throwDirection = dir;
        SetState(thrownState);
    }

    public void Move()
    {
        transform.position += throwDirection*projectileParams.Speed*Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.EnemyLayer)
        {
            ReturnObject();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.LevelBoundsLayer)
        {
            ReturnObject();
        }
    }

    void Update()
    {
        currentState.Update();
    }
}

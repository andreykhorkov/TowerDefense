using System;
using Pool;
using Projectiles;
using UnityEngine;
using VFX;

public class HitArgs : EventArgs
{
    public Collider Collider { get; private set; }

    public HitArgs(Collider collider)
    {
        Collider = collider;
    }
}

public class Projectile : PooledBattleElement
{
    [SerializeField] private ProjectileParams projectileParams;
    [SerializeField, AssetPathGetter] private string explosionAssetPath;

    private SphereCollider sphereCollider;
    private TrailRenderer trailRenderer;
    private IdleState idleState;
    private ThrownState thrownState;
    private ProjectileState currentState;
    private Vector3 throwDirection;

    public static event EventHandler<HitArgs> HitEnemyHandler = delegate { };

    public ProjectileParams ProjectileParams { get { return projectileParams; } }

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
        HelpTools.ChangeLayersRecursively(transform, BattleController.PoolLayer);
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
        gameObject.layer = BattleController.ProjectileLayer;
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
        if (collider.gameObject.layer != BattleController.EnemyLayer)
        {
            return;
        }

        ReturnObject();
        HitEnemyHandler(this, new HitArgs(collider));

        var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath);
        explosion.SetOrientation(collider.transform.position, Quaternion.identity);
        explosion.Play();
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

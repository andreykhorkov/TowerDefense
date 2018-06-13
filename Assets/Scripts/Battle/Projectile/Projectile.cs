using System;
using Pool;
using Projectiles;
using UnityEngine;
using VFX;

public class HitEnemyColArgs : EventArgs
{
    public Collider Collider { get; private set; }

    public HitEnemyColArgs(Collider collider)
    {
        Collider = collider;
    }
}

public class HitEnemyArgs : EventArgs
{
    public int Id { get; private set; }
    public int Damage { get; private set; }

    public HitEnemyArgs(int id, int damage)
    {
        Id = id;
        Damage = damage;
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
    private TurretController turretController;
    private EnemyController target;

    public static event EventHandler<HitEnemyColArgs> HitEnemyColliderHandler = delegate { };

    public static event EventHandler<HitEnemyArgs> HitEnemyHandler = delegate { };

    public ProjectileParams ProjectileParams { get { return projectileParams; } }

    protected override void Initialize()
    {
        base.Initialize();

        sphereCollider = GetComponent<SphereCollider>();
        trailRenderer = GetComponent<TrailRenderer>();
        turretController = BattleRoot.BattleController.TurretController;

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

    public void SetTarget(EnemyController target)
    {
        this.target = target;
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
        HitEnemyColliderHandler(this, new HitEnemyColArgs(collider));

        var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath);
        explosion.transform.position = collider.transform.position;
        explosion.Play();
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == BattleController.LevelBoundsLayer)
        {
            ReturnObject();
            HitEnemyHandler(this, new HitEnemyArgs(target.Id, projectileParams.Damage));

            var explosion = PoolManager.GetObject<ParticleEffect>(explosionAssetPath);
            explosion.SetOrientation(target.transform.position, Quaternion.identity);
            explosion.Play();
        }
    }

    void Update()
    {
        currentState.Update();
    }
}

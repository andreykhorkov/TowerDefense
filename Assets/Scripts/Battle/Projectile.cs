using UnityEngine;

public class Projectile : PooledBattleElement
{
    [SerializeField] private ProjectileParams projectileParams;

    public ProjectileParams ProjectileParams { get { return projectileParams; } }

    private SphereCollider sphereCollider;
    private TrailRenderer trailRenderer;

    protected override void Initialize()
    {
        base.Initialize();

        sphereCollider = GetComponent<SphereCollider>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public override void OnReturnedToPool()
    {
        sphereCollider.enabled = false;
        trailRenderer.enabled = false;
        SetOrientation(BattleController.PooledPosition, Quaternion.identity);
    }

    public override void OnTakenFromPool()
    {
        sphereCollider.enabled = true;
        trailRenderer.enabled = true;
    }
}

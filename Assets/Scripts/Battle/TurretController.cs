using Battle;
using Pool;
using UnityEngine;
using VFX;

public class TurretController : BattleElement
{

    [SerializeField] private Transform cannon;
    [SerializeField, Tooltip("degrees/sec")] private float maxTurretRotationSpeed = 360;
    [Header("20th part of levelBounds side")]
    [SerializeField, Range(1, 10)] private float targetingRadiusFraction = 5;
    [SerializeField, Range(0.2f, 0.8f), Tooltip("msec")] private float fireDelay = 0.5f;
    [SerializeField, AssetPathGetter] private string projectileAssetPath;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private ProjectileParams projectileParams;

    private EnemyController target;
    private BattleController battleController;
    private float levelHalfWidth;
    private int enemyLayerMask;
    private Collider[] colsWithinTargetingRadius = new Collider[100];
    private Vector3 turretPosition;
    private Vector3 targetDirection;
    private PeriodicTask shootingTask;

    public ProjectileParams ProjectileParams { get { return projectileParams; } }

    void Update()
    {
        Aiming();

        shootingTask.TryExecute();
    }

    protected override void Initialize()
    {
        base.Initialize();

        battleController = BattleRoot.BattleController;
        levelHalfWidth = battleController.LevelMesh.mesh.bounds.size.x*battleController.LevelMesh.transform.lossyScale.x;
        enemyLayerMask = LayerMask.GetMask("Enemy");
        turretPosition = transform.position;

        shootingTask = new PeriodicTask(Fire, fireDelay);

        PoolManager.PreWarm<Projectile>(projectileAssetPath, 2);
        
    }

    private void Fire()
    {
        if (target != null)
        {
            var projectile = PoolManager.GetObject<Projectile>(projectileAssetPath);
            projectile.SetOrientation(shootPoint.position, Quaternion.identity);
            projectile.Throw(targetDirection);
        }

        ChooseTarget();
    }

    private void ChooseTarget()
    {
        var targetingRadius = levelHalfWidth * targetingRadiusFraction/20;

        if (!Physics.CheckSphere(turretPosition, targetingRadius, enemyLayerMask))
        {
            return;
        }

        var numColsFound = Physics.OverlapSphereNonAlloc(turretPosition, targetingRadius, colsWithinTargetingRadius, enemyLayerMask);
        target = GetFurthestEnemy(numColsFound);
    }

    private EnemyController GetFurthestEnemy(int numColsFound)
    {
        var furthestDist = 0f;
        var furthestEnemyCollider = colsWithinTargetingRadius[0]; 

        for (int i = 0; i < numColsFound; i++)
        {
            var col = colsWithinTargetingRadius[i];
            var dist = (turretPosition - col.transform.position).sqrMagnitude;

            if (dist > furthestDist)
            {
                furthestEnemyCollider = col;
                furthestDist = dist;
            }
        }

        return furthestEnemyCollider.gameObject.GetComponent<EnemyController>();
    }

    private void Aiming()
    {
        if (ReferenceEquals(target, null))
        {
            return;
        }

        var dir = target.transform.position - cannon.position;
        var dist = Vector3.Distance(shootPoint.position, target.transform.position);
        var timeToHit = dist / projectileParams.Speed;
        var correction = timeToHit * target.transform.forward * target.Speed;
        var correctedDirection = dir + correction;

        targetDirection = correctedDirection.normalized;
        
        

        AimTurret();
        AimCannon();
    }

    private void AimTurret()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection, Vector3.up), maxTurretRotationSpeed * Time.deltaTime);
    }

    private void AimCannon()
    {
        var projection = Vector3.ProjectOnPlane(targetDirection, transform.right);
        cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, Quaternion.LookRotation(projection, Vector3.up), maxTurretRotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, targetDirection*20);
    }
}

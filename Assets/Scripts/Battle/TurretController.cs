using System.Collections;
using Battle;
using Pool;
using UnityEngine;
using Zenject;

public class TurretController : MonoBehaviour
{

    [SerializeField] private Transform cannon;
    [SerializeField, Tooltip("degrees/sec")] private float maxTurretRotationSpeed = 360;

    [Header("20th part of levelBounds side")] 
    [SerializeField, Range(1, 10)] private float targetingRadiusFraction = 5;
    [SerializeField, Range(0.2f, 0.8f), Tooltip("msec")] private float fireDelay = 0.5f;

    [Header("change offline only")]
    [SerializeField, Range(0.1f, 1)] private float enemyFindingDelay = 1;
    [SerializeField, AssetPathGetter] private string projectileAssetPath;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform towerBottom;
    [SerializeField] private ProjectileParams projectileParams;
    [Inject] private Projectile.Factory factory;

    private EnemyController target;
    private BattleController battleController;
    private float levelHalfWidth;
    private int enemyLayerMask;
    private Collider[] colsWithinTargetingRadius = new Collider[100];
    private Vector3 towerBottomPosition;
    private Vector3 targetDirection;
    private PeriodicTask choosingTarget;
    private bool isWeaponReady = true;
    private float targetingRadius;

    public Transform Target { get { return target.transform; } }

    public ProjectileParams ProjectileParams { get { return projectileParams; } }

    void Update()
    {
        Aiming();
        choosingTarget.TryExecute();
    }

    void OnDestroy()
    {
        EnemyController.EnemyDestroyed -= OnEnemyDestroyed;
    }

    [Inject]
    private void Initialize(BattleController battleController)
    {
        this.battleController = battleController;
        levelHalfWidth = battleController.LevelMesh.mesh.bounds.size.x*battleController.LevelMesh.transform.lossyScale.x;
        enemyLayerMask |= 1 << LayerMask.NameToLayer("Enemy");
        towerBottomPosition = towerBottom.transform.position;
        choosingTarget = new PeriodicTask(ChooseTarget, enemyFindingDelay);

        EnemyController.EnemyDestroyed += OnEnemyDestroyed;
    }

    private void Fire()
    {
        var projectile = PoolManager.GetObject<Projectile>(projectileAssetPath, factory);
        projectile.SetOrientation(shootPoint.position, shootPoint.rotation);
        projectile.SetTarget(target);
        projectile.Throw(targetDirection);

        StartCoroutine(Reloading());
    }

    private void ChooseTarget()
    {
        targetingRadius = levelHalfWidth * targetingRadiusFraction/20;

        if (!Physics.CheckSphere(towerBottomPosition, targetingRadius, enemyLayerMask))
        {
            return;
        }

        var numColsFound = Physics.OverlapSphereNonAlloc(towerBottomPosition, targetingRadius, colsWithinTargetingRadius, enemyLayerMask);
        target = GetFurthestEnemy(numColsFound);
    }

    private EnemyController GetFurthestEnemy(int numColsFound)
    {
        var furthestDist = 0f;
        var furthestEnemyCollider = colsWithinTargetingRadius[0]; 

        for (int i = 0; i < numColsFound; i++)
        {
            var col = colsWithinTargetingRadius[i];
            var dist = Vector3.SqrMagnitude(towerBottomPosition - col.transform.position);

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

        var isTargetAimed = HelpTools.Approximately(Vector3.Dot(targetDirection, cannon.transform.forward), 1, 0.001f);

        if (isTargetAimed && isWeaponReady)
        {
            Fire();
        }
    }

    private void OnEnemyDestroyed(object sender, EnemyArgs args)
    {
        if (target != null && args.Id == target.Id)
        {
            target = null;
        }
    }

    private IEnumerator Reloading()
    {
        isWeaponReady = false;
        yield return new WaitForSeconds(fireDelay);
        isWeaponReady = true;
    }

    private void AimTurret()
    {
        var isEnemyInFront = Vector3.Dot(targetDirection, transform.forward) > 0;
        var dotProd = Vector3.Dot(targetDirection, transform.right);
        float engineValue;

        if (isEnemyInFront)
        {
            engineValue = dotProd;
        }
        else
        {
            engineValue = dotProd > 0 ? 1 : -1;
        }

        transform.rotation *= Quaternion.AngleAxis(engineValue * maxTurretRotationSpeed * Time.deltaTime, transform.up);
    }

    private void AimCannon()
    {
        var projection = Vector3.ProjectOnPlane(targetDirection, transform.right);
        cannon.transform.rotation *= Quaternion.AngleAxis(Vector3.Dot(projection, -cannon.transform.up) * maxTurretRotationSpeed * Time.deltaTime, Vector3.right);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cannon.transform.position, targetDirection*20);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(towerBottomPosition, transform.forward * targetingRadius);
    }
}

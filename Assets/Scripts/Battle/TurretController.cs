using Battle;
using UnityEngine;

public class TurretController : BattleElement
{

    [SerializeField] private Transform cannon;
    [SerializeField, Tooltip("degrees/sec")] private float maxTurretRotationSpeed = 360;
    [Header("20th part of levelBounds side")]
    [SerializeField, Range(1, 10)] private float targetingRadiusFraction = 5;
    [SerializeField, Range(0.2f, 0.8f), Tooltip("msec")] private float fireDelay = 0.5f;

    private EnemyController target;
    private BattleController battleController;
    private float levelHalfWidth;
    private int enemyLayerMask;
    private Collider[] colsWithinTargetingRadius = new Collider[100];
    private Vector3 turretPosition;
    private PeriodicTask shootingTask;

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
    }

    private void Fire()
    {
        if (target != null)
        {
            Debug.LogFormat("Fire to {0}", target.Id);
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

        var dir = (target.transform.position - cannon.position).normalized;

        AimTurret(dir);
        AimCannon(dir);
    }

    private void AimTurret(Vector3 dir)
    {
        var isEnemyInFront = Vector3.Dot(dir, transform.forward) > 0;
        var dotProd = Vector3.Dot(dir, transform.right);
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

    private void AimCannon(Vector3 dir)
    {
        var projection = Vector3.ProjectOnPlane(dir, transform.right);
        cannon.transform.rotation *= Quaternion.AngleAxis(Vector3.Dot(projection, -cannon.transform.up) * maxTurretRotationSpeed * Time.deltaTime, Vector3.right);
    }
}

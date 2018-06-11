using Battle;
using Pool;
using UnityEngine;

public class BattleController : BattleElement
{
    [SerializeField] private Transform goalPoint;
    [SerializeField] private float defaultEnemySpawnDelay = 5; 
    [SerializeField] private float enemySpawnDecreaseStep = 0.05f;
    [SerializeField] private float enemySpeedIncreaseStep = 0.05f;
    [SerializeField] private float enemySpawnDecreasingDelay = 2;
    [SerializeField] private MeshFilter levelMesh;

    private EnemySpawnController enemySpawnController;
    private PeriodicTask spawnEnemiesTask;
    private PeriodicTask decreasingEnemySpawnDelay;
    private float additionSpeed;

    public static int EnemyLayer { get; private set; }

    public static int LevelBoundsLayer { get; private set; }

    public static int ProjectileLayer { get; private set; }

    public static Vector3 PooledPosition { get; private set; }
    
    public static int LastEnemyId { get; private set; }

    public static Vector3 GoalPosition { get; private set; }

    public MeshFilter LevelMesh { get { return levelMesh; } }

    public float EnemySpawnDelay { get; private set; }

    private void SetPeriodicTasks()
    {
        EnemySpawnDelay = defaultEnemySpawnDelay;
        spawnEnemiesTask = new PeriodicTask(SpawnEnemy, EnemySpawnDelay);
        decreasingEnemySpawnDelay = new PeriodicTask(Complication, enemySpawnDecreasingDelay);
    }

    private void SpawnEnemy()
    {
        var enemy = enemySpawnController.SpawnEnemy();
        enemy.AddSpeed(additionSpeed);
        StartCoroutine(enemy.SetGoalDestination());
    }

    private void Complication()
    {
        EnemySpawnDelay = Mathf.Clamp(EnemySpawnDelay - enemySpawnDecreaseStep, enemySpawnDecreaseStep, float.MaxValue);
        additionSpeed += enemySpeedIncreaseStep;
        spawnEnemiesTask.SetDelay(EnemySpawnDelay);
    }

    private void OnEnemyInstantiated(object sender, EnemyArgs args)
    {
        LastEnemyId++;
    }

    void Update()
    {
        spawnEnemiesTask.TryExecute();
        decreasingEnemySpawnDelay.TryExecute();
    }

    void OnDestroy()
    {
        EnemyController.EnemyInstantiated -= OnEnemyInstantiated;
    }

    protected override void Initialize()
    {
        base.Initialize();

        GoalPosition = goalPoint.position;
        enemySpawnController = BattleRoot.EnemySpawnController;
        SetPeriodicTasks();
        LastEnemyId = -1;
        PooledPosition = PoolManager.Instance.transform.position;
        EnemyController.EnemyInstantiated += OnEnemyInstantiated;
        LevelBoundsLayer = LayerMask.NameToLayer("LevelBounds");
        ProjectileLayer = LayerMask.NameToLayer("Projectile");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }
}

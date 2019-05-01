using System;
using Battle;
using Pool;
using UnityEngine;

public class ClockTickArgs : EventArgs
{
    public int Seconds { get; }

    public ClockTickArgs(int seconds)
    {
        Seconds = seconds;
    }
}

public class BattleController : BattleElement
{
    [SerializeField] private Transform goalPoint;
    [SerializeField] private float defaultEnemySpawnDelay = 5; 
    [SerializeField] private float enemySpawnDelayDecreaseStep = 0.1f;
    [SerializeField] private float enemySpeedIncreaseStep = 0.05f;
    [SerializeField] private float enemySpawnDecreasingDelay = 2;
    [SerializeField] private MeshFilter levelMesh;
    [SerializeField] private TurretController turretController;

    private EnemySpawnController enemySpawnController;
    private PeriodicTask spawnEnemiesTask;
    private PeriodicTask decreasingEnemySpawnDelay;
    private PeriodicTask clockTick;
    private int timer;
    private float additionSpeed;

    public static event EventHandler<ClockTickArgs> ClockTick = delegate { };

    public TurretController TurretController { get { return turretController; } }

    public BattleStats BattleStats { get; private set; }

    public static int EnemyLayer { get; private set; }

    public static int LevelBoundsLayer { get; private set; }

    public static int FinishLayer { get; private set; }

    public static int ProjectileLayer { get; private set; }

    public static int PoolLayer { get; private set; }

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
        clockTick = new PeriodicTask(Tick, 1);
    }

    private void Tick()
    {
        timer++;
        ClockTick(this, new ClockTickArgs(timer));
    }

    private void SpawnEnemy()
    {
        var enemy = enemySpawnController.SpawnEnemy();
        enemy.AddSpeed(additionSpeed);
        enemy.SetGoalDestination();
    }

    private void Complication()
    {
        EnemySpawnDelay = Mathf.Clamp(EnemySpawnDelay - enemySpawnDelayDecreaseStep, enemySpawnDelayDecreaseStep, float.MaxValue);
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
        clockTick.TryExecute();
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
        FinishLayer = LayerMask.NameToLayer("Finish");
        ProjectileLayer = LayerMask.NameToLayer("Projectile");
        PoolLayer = LayerMask.NameToLayer("Pool");
        EnemyLayer = LayerMask.NameToLayer("Enemy");

        BattleStats = new BattleStats();
    }
}

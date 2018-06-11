using System;
using Battle;
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
    private float enemySpawnDelay;
    private float additionSpeed;
    
    public int LastEnemyId { get; private set; }

    public MeshFilter LevelMesh { get { return levelMesh; } }

    public Vector3 GoalPosition { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        GoalPosition = goalPoint.position;
        enemySpawnController = BattleRoot.EnemySpawnController;
        SetPeriodicTasks();
        LastEnemyId = -1;
        EnemyController.EnemyInstantiated += OnEnemyInstantiated;
    }

    private void OnDestroy()
    {
        EnemyController.EnemyInstantiated -= OnEnemyInstantiated;
    }

    private void OnEnemyInstantiated(object sender, EnemyArgs args)
    {
        LastEnemyId = args.Id;
    }

    private void SetPeriodicTasks()
    {
        enemySpawnDelay = defaultEnemySpawnDelay;
        spawnEnemiesTask = new PeriodicTask(SpawnEnemy, enemySpawnDelay);
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
        enemySpawnDelay = Mathf.Clamp(enemySpawnDelay - enemySpawnDecreaseStep, enemySpawnDecreaseStep, float.MaxValue);
        additionSpeed += enemySpeedIncreaseStep;
        spawnEnemiesTask.SetDelay(enemySpawnDelay);
    }

    void Update()
    {
        spawnEnemiesTask.TryExecute();
        decreasingEnemySpawnDelay.TryExecute();

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    var enemy = enemySpawnController.SpawnEnemy();
        //    enemy.AddSpeed(0);
        //    StartCoroutine(enemy.SetGoalDestination());
        //}
    }
}

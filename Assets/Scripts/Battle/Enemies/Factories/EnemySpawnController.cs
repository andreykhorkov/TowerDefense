using Battle;
using Pool;
using UnityEngine;

public class EnemySpawnController : BattleElement
{

    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;
    [SerializeField] private EnemyFactory simpleEnemyFactory;
    [SerializeField] private EnemyFactory advancedEnemyFactory;
    private Vector3 startLine;

    protected override void Initialize()
    {
        base.Initialize();

        startLine = rightWing.position - leftWing.position;
    }

    public EnemyController SpawnEnemy()
    {
        var currentEnemyFactory = Random.Range(0, 100) > 50 ? simpleEnemyFactory : advancedEnemyFactory;

        var enemy = currentEnemyFactory.SpawnEnemy();
        enemy.transform.position = leftWing.position + startLine.normalized * Random.Range(0, startLine.magnitude);
        return enemy;
    }
}

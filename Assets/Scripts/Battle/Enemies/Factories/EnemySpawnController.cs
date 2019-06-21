using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;
    [SerializeField] private EnemyFactory simpleEnemyFactory;
    [SerializeField] private EnemyFactory advancedEnemyFactory;
    private Vector3 startLine;

    protected void Initialize()
    {
        startLine = rightWing.position - leftWing.position;
    }

    public EnemyController SpawnEnemy()
    {
        var currentEnemyFactory = Random.Range(0, 100) > 50 ? simpleEnemyFactory : advancedEnemyFactory;

        var enemy = currentEnemyFactory.SpawnEnemy();
        return enemy;
    }

    public Vector3 GetSpawnPos()
    {
        return leftWing.position + startLine.normalized * Random.Range(0, startLine.magnitude);
    }
}

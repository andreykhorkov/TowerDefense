using Pool;

public class SimpleEnemyFactory : EnemyFactory
{
    public override EnemyController SpawnEnemy()
    {
        return PoolManager.GetObject<EnemyController>(enemyAssetPath);
    }
}

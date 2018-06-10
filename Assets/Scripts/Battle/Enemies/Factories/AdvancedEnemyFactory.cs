using Pool;
using UnityEngine;

public class AdvancedEnemyFactory : EnemyFactory
{
    [SerializeField, AssetPathGetter] protected string enemyViewPath;


    public override EnemyController SpawnEnemy()
    {
        throw new System.NotImplementedException();
    }
}

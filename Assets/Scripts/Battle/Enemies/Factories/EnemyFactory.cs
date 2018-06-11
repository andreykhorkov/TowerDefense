using Battle;
using Pool;
using UnityEngine;

public abstract class EnemyFactory : BattleElement
{
    [SerializeField, AssetPathGetter] protected string enemyAssetPath;

    public abstract EnemyController SpawnEnemy();

    protected override void Initialize()
    {
        base.Initialize();

        PoolManager.PreWarm<EnemyController>(enemyAssetPath, 20);
    }
}

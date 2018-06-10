using Battle;
using UnityEngine;

public abstract class EnemyFactory : BattleElement
{
    [SerializeField, AssetPathGetter] protected string enemyAssetPath;

    public abstract EnemyController SpawnEnemy();
}

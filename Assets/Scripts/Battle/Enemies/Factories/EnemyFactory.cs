using Pool;
using UnityEngine;
using Zenject;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField, AssetPathGetter] protected string enemyAssetPath;
    [Inject] private EnemyController.Factory factory;

    public EnemyController SpawnEnemy()
    {
        return PoolManager.GetObject<EnemyController>(enemyAssetPath, factory);//   factory.Create(enemyAssetPath);
    }

    protected void Initialize()
    {
        PoolManager.PreWarm<EnemyController>(enemyAssetPath, 20, factory);
    }
}

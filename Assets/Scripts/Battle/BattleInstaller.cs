using UnityEngine;
using VFX;
using Zenject;

public class BattleInstaller : MonoInstaller
{
    [SerializeField] private EnemySpawnController enemySpawner;
    [SerializeField] private BattleController battleController;
    [SerializeField] private TurretController turretController;

    public override void InstallBindings()
    {
        Container.BindInstance(battleController);
        Container.BindInstance(enemySpawner);
        Container.BindInstance(turretController);
        Container.Bind<BattleStats>().AsSingle();

        Container.BindFactory<string, EnemyController, EnemyController.Factory>()
            .FromFactory<PrefabResourceFactory<EnemyController>>();
        Container.BindFactory<string, Projectile, Projectile.Factory>()
            .FromFactory<PrefabResourceFactory<Projectile>>();
        Container.BindFactory<string, ParticleEffect, ParticleEffect.Factory>()
            .FromFactory<PrefabResourceFactory<ParticleEffect>>();
    }
}
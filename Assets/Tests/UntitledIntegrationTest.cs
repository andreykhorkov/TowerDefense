using Zenject;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using VFX;

public class UntitledIntegrationTest : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator RunTest1()
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc

        PreInstall();

        // Call Container.Bind methods
        Container.Bind<SimpleEnemyController>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<EnemySpawnController>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<BattleController>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<Transform>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<BattleStats>().AsSingle();

        Container.BindFactory<string, EnemyController, EnemyController.Factory>()
            .FromFactory<PrefabResourceFactory<EnemyController>>();
        Container.BindFactory<string, Projectile, Projectile.Factory>()
            .FromFactory<PrefabResourceFactory<Projectile>>();
        Container.BindFactory<string, ParticleEffect, ParticleEffect.Factory>()
            .FromFactory<PrefabResourceFactory<ParticleEffect>>();

        var enemy = Container.Resolve<SimpleEnemyController>();
        Debug.Log(enemy);

        var prefab = Resources.Load<EnemyController>("EnemyVehicles/SimpleEnemy");
        var suka = Object.Instantiate(prefab);

        PostInstall();

        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield return null;

        var foundEnemy = Object.FindObjectOfType<EnemyController>();
        var originalPrefab = PrefabUtility.GetCorrespondingObjectFromSource(suka);
        Assert.IsTrue(foundEnemy is SimpleEnemyController);
    }
}
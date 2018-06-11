using UnityEngine;

namespace Battle
{
    public class BattleRoot : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private EnemySpawnController enemySpawnController;

        public BattleController BattleController { get { return battleController; } }
        public EnemySpawnController EnemySpawnController { get { return enemySpawnController; } }
       
    }
}

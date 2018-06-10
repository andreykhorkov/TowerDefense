using UnityEngine;

namespace Battle
{
    public class BattleElement : MonoBehaviour
    {
        public BattleRoot BattleRoot { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            BattleRoot = FindObjectOfType<BattleRoot>();
        }
    }
}

using UnityEngine;

namespace Battle
{
    public class BattleElement : MonoBehaviour
    {
        public BattleRoot BattleRoot { get; private set; }

        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            BattleRoot = FindObjectOfType<BattleRoot>();
        }
    }
}

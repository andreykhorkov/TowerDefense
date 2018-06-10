using Battle;
using Pool;

public class PooledBattleElement : PoolObject
{
    public BattleRoot BattleRoot { get; private set; }

    private void Start()
    {
        BattleRoot = FindObjectOfType<BattleRoot>();
    }

    public override void OnTakenFromPool()
    {
    }

    public override void OnReturnedToPool()
    {
    }

    public override void OnPreWarmed()
    {
    }
}

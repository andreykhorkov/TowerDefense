using System;

public class BattleStats
{
    public int SimpleEnemiesFrags { get; private set; }
    public int AdvancedEnemiesFrags { get; private set; }

    public static event EventHandler StatsChanged = delegate { };

    public void CountFrag(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.simple:
                SimpleEnemiesFrags++;
                break;
            case EnemyType.advanced:
                AdvancedEnemiesFrags++;
                break;
        }

        StatsChanged(this, EventArgs.Empty);
    }

    public void ResetStats()
    {
        SimpleEnemiesFrags = 0;
        AdvancedEnemiesFrags = 0;
        StatsChanged(this, EventArgs.Empty);
    }
}

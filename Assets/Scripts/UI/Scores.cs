using System;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class Scores : BattleElement
{
    [SerializeField] private Text numSimple;
    [SerializeField] private Text numAdvanced;

    private BattleStats battleStats;

    protected override void Initialize()
    {
        base.Initialize();

        BattleStats.StatsChanged += OnStatsChanged;
        battleStats = BattleRoot.BattleController.BattleStats;
        battleStats.ResetStats();
    }

    void OnDestroy()
    {
        BattleStats.StatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(object sender, EventArgs args)
    {
        numSimple.text = battleStats.SimpleEnemiesFrags.ToString();
        numAdvanced.text = battleStats.AdvancedEnemiesFrags.ToString();
    }

}

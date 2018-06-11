using System;
using System.Text;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class Scores : BattleElement
{
    [SerializeField] private Text numSimple;
    [SerializeField] private Text numAdvanced;
    [SerializeField] private Text timer;

    private BattleStats battleStats;

    protected override void Initialize()
    {
        base.Initialize();

        BattleStats.StatsChanged += OnStatsChanged;
        BattleController.ClockTick += OnClockTick;
        battleStats = BattleRoot.BattleController.BattleStats;
        battleStats.ResetStats();
    }

    void OnDestroy()
    {
        BattleStats.StatsChanged -= OnStatsChanged;
        BattleController.ClockTick -= OnClockTick;
    }

    private void OnStatsChanged(object sender, EventArgs args)
    {
        numSimple.text = battleStats.SimpleEnemiesFrags.ToString();
        numAdvanced.text = battleStats.AdvancedEnemiesFrags.ToString();
    }

    private void OnClockTick(object sender, ClockTickArgs args)
    {
        var t = args.Seconds;
        var min =  t/60;
        var sec = t%60;

        timer.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

}

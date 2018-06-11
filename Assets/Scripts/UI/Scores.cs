using System;
using System.Text;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class Scores : BattleElement
{
    private const int SECONDS_IN_MIN = 60;

    [SerializeField] private Text numSimple;
    [SerializeField] private Text numAdvanced;
    [SerializeField] private Text timer;

    private StringBuilder sb = new StringBuilder(5);

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
        sb.Remove(0, sb.Length);

        var t = args.Seconds;
        var min =  t/SECONDS_IN_MIN;
        var sec = t%SECONDS_IN_MIN;

        sb.AppendFormat("{0:D2}:{1:D2}", min, sec);

        timer.text = sb.ToString();
    }

}

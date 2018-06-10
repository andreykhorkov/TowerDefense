using System;
using UnityEngine;

public class PeriodicTask
{
    private float timer;
    private float delay;
    private Action callback;

    public PeriodicTask(Action callback, float delay)
    {
        this.callback = callback;
        SetDelay(delay);
    }

    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    public void TryExecute()
    {
        timer += Time.deltaTime;

        if (timer > delay)
        {
            Execute();
        }
    }

    public void Execute()
    {
        callback.Invoke();
        timer = 0;
    }
}
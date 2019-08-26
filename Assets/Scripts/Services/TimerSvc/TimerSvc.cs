using System;
 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class TimerSvc : BaseSystem
{
    public static TimerSvc Instance;

    private Timer timer;

    public void InitSvc()
    {
        Instance = this;
        timer = new Timer();
        timer.SetLog(info => { CommonTool.Log(info); });
        CommonTool.Log("TimerSvc Connected");
    }

    private void Update()
    {
        timer.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, TimeUnit timeunit = TimeUnit.Millisecond, int count = 1)
    {
        return timer.AddTimeTask(callback, delay, timeunit, count);
    }

    public double GetNowTime()
    {
        return timer.GetMillisecondsTime();
    }

}
using UnityEngine;
using UnityEngine.Events;

public class CounterTime
{
    UnityAction doneAction;
    private float time;
    private float TIME;

    private float timeDelay;
    private float TimeDelay;
    private bool isLoop;
    private bool isDelayTime = false;
    public bool IsRunning => time > 0;

    public void Start(UnityAction doneAction, float time, bool isLoop = false, float timeDelay = 0f)
    {
        this.doneAction = doneAction;
        this.TIME = time;
        this.time = time;
        this.isLoop = isLoop;
        this.TimeDelay = timeDelay;
        this.timeDelay = timeDelay;
    }

    public void Execute()
    {
        if (isLoop)
        {
            if (isDelayTime == true)
            {
                if (timeDelay > 0)
                {
                    timeDelay -= Time.deltaTime;
                    if (timeDelay <= 0)
                    {
                        isDelayTime = false;
                        timeDelay = TimeDelay;
                    }
                }
            }
            else
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                    if (time <= 0)
                    {
                        Exit();
                    }
                }
            }
        }
        else
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    Exit();
                }
            }
        }


    }

    public void Exit()
    {
        doneAction?.Invoke();
        if (isLoop == true)
        {
            Reset();
        }
    }

    public void Cancel()
    {
        doneAction = null;
        time = -1;
        isDelayTime = false;
        timeDelay = -1;
    }

    public void Reset()
    {
        time = this.TIME;
        isDelayTime = true;
    }
}
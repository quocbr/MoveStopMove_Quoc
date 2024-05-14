using UnityEngine;
using UnityEngine.Events;

public class CounterTime
{
    UnityAction doneAction;
    private float time;
    public bool IsRunning => time > 0;

    public void Start(UnityAction doneAction, float time)
    {
        this.doneAction = doneAction;
        this.time = time;
    }

    public void Execute()
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

    public void Exit()
    {
        doneAction?.Invoke();
    }

    public void Cancel()
    {
        doneAction = null;
        time = -1;
    }
}
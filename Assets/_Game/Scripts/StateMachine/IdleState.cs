using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class IdleState : IState<Character>
{
    float timer = 0;
    float timeDelay;
    public void OnEnter(Character t)
    {
        t.StopMoving();
        (t as Bot).Counter.Start(() => t.ChangeState(new PatrolState()), Random.Range(0f, 2f));
    }

    public void OnExecute(Character t)
    {
        (t as Bot).Counter.Execute();
    }

    public void OnExit(Character t)
    {

    }

}

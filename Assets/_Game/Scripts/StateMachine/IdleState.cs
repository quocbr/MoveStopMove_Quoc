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
        (t as Bot).StopMoving();
        timeDelay = Random.Range(1f, 3f);
    }

    public void OnExecute(Character t)
    {
        //UNDONE:
        if(timer > timeDelay)
        {
            (t as Bot).ChangeState(new PatrolState());
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    public void OnExit(Character t)
    {

    }

}

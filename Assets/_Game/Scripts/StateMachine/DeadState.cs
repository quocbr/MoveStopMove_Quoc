using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<Character>
{
    public void OnEnter(Character t)
    {
        (t as Bot).HandleBeginDead();
    }

    public void OnExecute(Character t)
    {

    }

    public void OnExit(Character t)
    {

    }

}

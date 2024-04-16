using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Character>
{
    public void OnEnter(Character t)
    {
        t.StopMoving();
    }

    public void OnExecute(Character t)
    {
        t.Attack();

        if(t.CheckAttack() == false)
        {
            t.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Character t)
    {
    }

}

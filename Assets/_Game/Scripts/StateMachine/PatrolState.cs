using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Character>
{
    public void OnEnter(Character t)
    {
        (t as Bot).Moving();
    }

    public void OnExecute(Character t)
    {
        if((t as Bot).CheckAttack())
        {
            (t as Bot).ChangeState(new AttackState());
        }

        if((t as Bot).IsMoveFinish()) 
        {
            (t as Bot).ChangeState(new IdleState());
        }

        //(t as Bot).Patrol();
    }

    public void OnExit(Character t)
    {

    }

}

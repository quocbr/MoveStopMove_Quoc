using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Character>
{
    public void OnEnter(Character t)
    {
        (t as Bot).SetDestination(LevelManager.Ins.RandomPoint());
        //t.Moving();
    }

    public void OnExecute(Character t)
    {
        //if((t as Bot).CheckAttack())
        //{
        //    (t as Bot).ChangeState(new AttackState());
        //}

        if ((t as Bot).IsDestination)
        {
            t.ChangeState(new IdleState());
        }

    }

    public void OnExit(Character t)
    {

    }

}

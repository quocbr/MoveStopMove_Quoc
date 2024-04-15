using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Character>
{
    public void OnEnter(Character t)
    {
        (t as Bot).StopMoving();
    }

    public void OnExecute(Character t)
    {
        (t as Bot).BotAttack();

        if((t as Bot).CheckAttack() == false)
        {
            (t as Bot).ChangeState(new PatrolState());
        }
    }

    public void OnExit(Character t)
    {
    }

}

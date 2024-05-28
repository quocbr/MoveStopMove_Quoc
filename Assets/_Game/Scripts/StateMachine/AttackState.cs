using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Character>
{
    public void OnEnter(Character t)
    {
        t.StopMoving();
        (t as Bot).Counter.Start(() =>
        {
            t.OnAttack();
            if (t.IsCanAttack)
            {
                (t as Bot).Counter.Start(
                    () =>
                    {
                        t.Throw();
                        (t as Bot).Counter.Start(
                        () =>
                        {
                            t.ChangeState(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState());

                        }, Character.TIME_DELAY_THROW);
                    }, Character.TIME_DELAY_THROW
                );
            }
        }, Random.Range(0.2f, 0.75f)
        );
    }

    public void OnExecute(Character t)
    {
        (t as Bot).Counter.Execute();
    }

    public void OnExit(Character t)
    {
    }

}

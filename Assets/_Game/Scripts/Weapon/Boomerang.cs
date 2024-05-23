using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Boomerang : Bullet
{
    public float Time_back = 1f;
    public enum State { Forward, Backward, Stop }

    private State state;

    //[SerializeField] Transform child;

    //CounterTime counterTime = new CounterTime();

    public override void OnInit(Material material, Character attacker, Action<Character, Character> onHit)
    {
        base.OnInit(material, attacker, onHit);
        counterTime.Start(Back, Time_back * attacker.Size);
        state = State.Forward;
    }
    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Forward:
                //TF.position = Vector3.MoveTowards(TF.position, this.target, moveSpeed * Time.deltaTime);
                TF.Translate(TF.forward * currentMoveSpeed * Time.deltaTime, Space.World);
                counterTime.Execute();
                child.Rotate(Vector3.up * -6, Space.Self);
                break;

            case State.Backward:
                TF.position = Vector3.MoveTowards(TF.position, this.attacker.TF.position, currentMoveSpeed * Time.deltaTime);
                if (attacker.IsDead || Vector3.Distance(TF.position, this.attacker.TF.position) < 0.1f)
                {
                    OnDespawn();
                }
                child.Rotate(Vector3.up * -6, Space.Self);

                break;
        }
    }

    private void Back()
    {
        state = State.Backward;
    }

    protected override void OnStop()
    {
        base.OnStop();
        state = State.Stop;
        Invoke(nameof(OnDespawn), 2f);
    }

    //protected void BoomerangThrow()
    //{
    //    timer += Time.deltaTime;
    //    if(timer > timeBack && isBack == false) 
    //    {
    //        isBack = true;
    //        rb.velocity = Vector3.zero;
    //        moveVector = (TF.position - startPoit).normalized;
    //    }

    //    if(isBack == true) 
    //    {
    //        TF.position = Vector3.MoveTowards(TF.position,startPoit,moveSpeed*Time.deltaTime);
    //    }

    //    if(timer > 2f * timeBack)
    //    {
    //        OnDespawn();
    //    }

    //}

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(Constant.TAG_CHARACTER))
    //    {
    //        Character victim = Cache.GetCharacter(other);
    //        if (attacker.Equals(victim) && isBack)
    //        {
    //            OnDespawn();
    //            return;
    //        }

    //    }
    //    base.OnTriggerEnter(other);
    //}
}

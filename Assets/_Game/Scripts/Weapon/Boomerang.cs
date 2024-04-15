using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{
    [SerializeField]private float timeBack = 2f;
    private Vector3 startPoit;
    private float timer = 0;
    private bool isBack;

    private Vector3 moveVector;
    public override void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        timeLife = float.MaxValue;
        base.OnInit(attacker, onHit);
        transform.Rotate(90f, 0, 0);
        timer = 0f;
        startPoit = attacker.TF.position;
        isBack = false;
    }
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, 0, rotateSpeed);
        BoomerangThrow();
    }

    protected void BoomerangThrow()
    {
        timer += Time.deltaTime;
        if(timer > timeBack && isBack == false) 
        {
            isBack = true;
            rb.velocity = Vector3.zero;
            moveVector = (TF.position - startPoit).normalized;
        }

        if(isBack == true) 
        {
            TF.position = Vector3.MoveTowards(TF.position,startPoit,moveSpeed*Time.deltaTime);
        }

        if(timer > 2f * timeBack)
        {
            OnDespawn();
        }

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);
            if (attacker.Equals(victim) && isBack)
            {
                OnDespawn();
                return;
            }
            
        }
        base.OnTriggerEnter(other);
    }
}

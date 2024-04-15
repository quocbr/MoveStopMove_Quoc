using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float timeLife;
    [SerializeField] protected Rigidbody rb;

    protected Character attacker;
    protected Action<Character,Character> onHit;
    // set bullet data for bullet
    public virtual void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        TF.localScale = attacker.TF.localScale;
        Vector3 dir = new Vector3(attacker.AttackDir.x,0, attacker.AttackDir.z);
        rb.velocity = dir.normalized * moveSpeed;
        Invoke(nameof(OnDespawn), timeLife);
    }

    protected virtual void Update()
    {

    }

    public void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        HBPool.Despawn(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);
            if (attacker.Equals(victim) || victim.IsDead)
            {
                return;
            }
            onHit?.Invoke(attacker, victim);
            OnDespawn();
        }
    }

}

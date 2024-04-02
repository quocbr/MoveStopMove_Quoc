using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] protected float rotateSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeLife;
    [SerializeField] private Rigidbody rb;

    protected Character attacker;
    protected Action<Character,Character> onHit;
    // set bullet data for bullet
    public virtual void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        Vector3 dir = new Vector3(attacker.attackDir.x,0, attacker.attackDir.z);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);
            if (attacker.Equals(victim))
            {
                return;
            }
            onHit?.Invoke(attacker, victim);
            OnDespawn();
        }
    }

}

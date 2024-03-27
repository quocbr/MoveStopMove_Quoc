using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : GameUnit
{
    protected Character attacker;
    protected Action<Character,Character> onHit;
    // set bullet data for bullet
    public virtual void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        Invoke(nameof(OnDespawn), 3f);
    }

    Transform target;
    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, transform.forward, 2f * Time.deltaTime);
    }

    public void OnDespawn()
    {
        HBPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);
            onHit?.Invoke(attacker, victim);
            OnDespawn();
        }
    }

}

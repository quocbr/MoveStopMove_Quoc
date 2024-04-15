using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Bullet
{
    public override void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        Vector3 dir = new Vector3(attacker.AttackDir.x, 0, attacker.AttackDir.z);
        Quaternion rotation = Quaternion.LookRotation(dir);
        TF.rotation = rotation;
        transform.Rotate(-90f, transform.rotation.y, transform.rotation.z);
        rb.velocity = dir.normalized * moveSpeed;
        Invoke(nameof(OnDespawn), timeLife);

    }
    protected override void Update()
    {
        base.Update();
    }
}

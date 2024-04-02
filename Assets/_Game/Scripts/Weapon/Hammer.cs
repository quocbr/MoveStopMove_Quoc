using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Bullet
{
    public override void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        base.OnInit(attacker, onHit);
        transform.Rotate(90f, 0, 0);
    }
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0,0,rotateSpeed);
    }
}

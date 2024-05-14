using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : Bullet
{
    public const float TIME_ALIVE = 1f;

    //CounterTime counterTime = new CounterTime();

    public override void OnInit(Material material,Character attacker, Action<Character, Character> onHit)
    {

        base.OnInit(material, attacker, onHit);
        //transform.Rotate(90f, 0, 0);
        counterTime.Start(OnDespawn, TIME_ALIVE * attacker.Size);
    }
    protected override void Update()
    {
        base.Update();
        //transform.Rotate(0, 0, rotateSpeed);
        counterTime.Execute();
        if (isRunning)
        {
            //TF.Translate(TF.forward * moveSpeed * Time.deltaTime, Space.World);
            MoveRotate();
        }
    }

    protected override void OnStop()
    {
        base.OnStop();
        isRunning = false;
    }
}

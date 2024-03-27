using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public void Throw(Character character, Action<Character, Character> onHit)
    {
        Bullet bullet = HBPool.Spawn<Bullet>(PoolType.Bullet,character.throwPos.transform.position,Quaternion.identity);
        bullet.OnInit(character, onHit);
    }

}

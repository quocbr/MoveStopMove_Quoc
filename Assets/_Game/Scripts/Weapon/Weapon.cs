using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.VFX;


public class Weapon : GameUnit
{
    [SerializeField] private MeshRenderer meshRenderer;

    Bullet bullet;
    public void Throw(Character character, Action<Character, Character> onHit)
    {
        switch (poolType)
        {
            case PoolType.Axe:
                bullet = HBPool.Spawn<Axe>(PoolType.B_Axe, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Knife:
                bullet = HBPool.Spawn<Knife>(PoolType.B_Knife, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Boomerang:
                bullet = HBPool.Spawn<Boomerang>(PoolType.B_Boomerang, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Hammer:
                bullet = HBPool.Spawn<Hammer>(PoolType.B_Hammer, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_0:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_0, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_2:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_2, character.ThrowPos.transform.position, Quaternion.identity);
                break;
        }
        //Bullet bullet = HBPool.Spawn<Bullet>(PoolType.Bullet,character.throwPos.transform.position,Quaternion.identity);
        bullet.OnInit(character, onHit);
    }

    public void HideMesh(bool hide)
    {
        meshRenderer.enabled = hide;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.VFX;

public enum WeaponType
{
    Axe,
    Knife,
    Boomerang,
    Hammer,
    Candy_2
};

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    Bullet bullet;
    public void Throw(Character character, Action<Character, Character> onHit)
    {
        switch (weaponType)
        {
            case WeaponType.Axe:
                break;
            case WeaponType.Knife:
                break;
            case WeaponType.Boomerang:
                break;
            case WeaponType.Hammer:
                bullet = HBPool.Spawn<Hammer>(PoolType.Hammer, character.throwPos.transform.position, Quaternion.identity);
                break;
            case WeaponType.Candy_2:
                bullet = HBPool.Spawn<Candy>(PoolType.Candy_2, character.throwPos.transform.position, Quaternion.identity);
                break;
        }
        //Bullet bullet = HBPool.Spawn<Bullet>(PoolType.Bullet,character.throwPos.transform.position,Quaternion.identity);
        bullet.OnInit(character, onHit);
    }

}

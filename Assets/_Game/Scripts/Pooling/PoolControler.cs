using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PoolControler : MonoBehaviour
{
    [Space]
    [Header("Pool")]
    public List<GameUnit> PoolNoneRoot;

    [Header("Pool")]
    public List<PoolAmount> PoolWithRoot;

    [Header("Head")]
    public List<GameUnit> PoolHead;
    
    [Header("Wing")]
    public List<GameUnit> PoolWing;
    
    [Header("Tail")]
    public List<GameUnit> PoolTail;
    
    [Header("Shield")]
    public List<GameUnit> PoolShield;

    [Header("Weapon")]
    public List<GameUnit> PoolWeapon;

    [Header("Particle")]
    public ParticleAmount[] Particle;


    public void Awake()
    {
        for (int i = 0; i < PoolNoneRoot.Count; i++)
        {
            HBPool.Preload(PoolNoneRoot[i], 0, transform);
        }       
        
        for (int i = 0; i < PoolWithRoot.Count; i++)
        {
            HBPool.Preload(PoolWithRoot[i].prefab, PoolWithRoot[i].amount, PoolWithRoot[i].root);
        }

        for (int i = 0; i < PoolHead.Count; i++)
        {
            HBPool.Preload(PoolHead[i], 0, transform);
        }
        
        for (int i = 0; i < PoolTail.Count; i++)
        {
            HBPool.Preload(PoolTail[i], 0, transform);
        }
        
        for (int i = 0; i < PoolWing.Count; i++)
        {
            HBPool.Preload(PoolWing[i], 0, transform);
        }
        
        for (int i = 0; i < PoolShield.Count; i++)
        {
            HBPool.Preload(PoolShield[i], 0, transform);
        }
        
        for (int i = 0; i < PoolWeapon.Count; i++)
        {
            HBPool.Preload(PoolWeapon[i], 0, transform);
        }

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;

    public PoolAmount (Transform root, GameUnit prefab, int amount)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
    }
}


[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}




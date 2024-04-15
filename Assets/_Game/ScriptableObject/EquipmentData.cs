using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EquipmentData
{
    public int id;
    public string name;
    public string description;
    public EquipmentType equipmenyType;
    public PoolType poolType;
    public SetType setType;
    public List<Material> materials;
    public Texture2D image;
    public int value;
    public int cost;
}

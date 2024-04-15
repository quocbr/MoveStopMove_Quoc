using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    public List<EquipmentData> eWeapon;
    public List<EquipmentData> eHead;
    public List<EquipmentData> eWing;
    public List<EquipmentData> ePant;
    public List<EquipmentData> eTail;
    public List<EquipmentData> eShield;

    public List<EquipmentData> eSet;
}

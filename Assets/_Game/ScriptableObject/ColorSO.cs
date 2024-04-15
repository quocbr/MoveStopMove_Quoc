using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorSO")]
public class ColorSO : ScriptableObject
{
    public List<Material> colorList;

    public List<Material> colorSet;
}

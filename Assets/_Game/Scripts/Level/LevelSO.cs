using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SO", menuName = "ScriptableObjects/LevelData")]
public class LevelSO : ScriptableObject
{
    public List<LevelItemData> listLevelItems;
}

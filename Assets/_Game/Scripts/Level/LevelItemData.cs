using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class LevelItemData 
{
    public int levelIndex;
    public MapControler map;
    public NavMeshData navMeshData;
    public int aliveCount;
    public int botCount;
}

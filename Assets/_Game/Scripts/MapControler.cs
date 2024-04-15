using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControler : MonoBehaviour
{
    [SerializeField] private List<Transform> l_SpawnPos;

    public List<Transform> L_SpawnPos { get => l_SpawnPos;}
}

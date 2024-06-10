using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "ScriptableObjects/Setting")]
public class SettingSO : ScriptableObject
{
    public bool isMuteMusicAndSound;
    public bool isOffVibrate;
}

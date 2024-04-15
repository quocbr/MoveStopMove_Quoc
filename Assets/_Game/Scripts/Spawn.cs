using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private List<Character> l_Characters = new List<Character>();

    private void Update()
    {
        for (int i = l_Characters.Count - 1; i >= 0; i--)
        {
            if (l_Characters[i].IsDead)
            {
                l_Characters.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            l_Characters.Add(Cache.GetCharacter(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            l_Characters.Remove(Cache.GetCharacter(other));
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool IsSpawn()
    {
        return l_Characters.Count == 0;
    }
}

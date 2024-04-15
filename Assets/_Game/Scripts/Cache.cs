using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{

    private static Dictionary<Collider, Character> characters = new Dictionary<Collider, Character>();
    
    private static Dictionary<Transform, Spawn> spawns = new Dictionary<Transform, Spawn>();

    public static Character GetCharacter(Collider collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<Character>());
        }

        return characters[collider];
    }

    public static Spawn GetSpawn(Transform transform)
    {
        if (!spawns.ContainsKey(transform))
        {
            spawns.Add(transform, transform.GetComponent<Spawn>());
        }

        return spawns[transform];
    }
}


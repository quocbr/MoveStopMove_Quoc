using System.Collections.Generic;
using UnityEngine;

public class TargetCache
{
    static readonly Dictionary<int, Target2> cacheDict = new Dictionary<int, Target2>();
    public static Target2 Get(Component o)
    {
        int key = o.GetHashCode();
        if (!cacheDict.ContainsKey(key))
        {
            cacheDict.Add(key, o.GetComponent<Target2>());
        }
        return cacheDict[key];
    }
    public static void Clear()
    {
        cacheDict.Clear();
    }
}
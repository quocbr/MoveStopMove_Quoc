using System.Collections.Generic;
using UnityEngine;

public class TargetCache
{
    static readonly Dictionary<int, Target> cacheDict = new Dictionary<int, Target>();
    public static Target Get(Component o)
    {
        int key = o.GetHashCode();
        if (!cacheDict.ContainsKey(key))
        {
            cacheDict.Add(key, o.GetComponent<Target>());
        }
        return cacheDict[key];
    }
    public static void Clear()
    {
        cacheDict.Clear();
    }
}
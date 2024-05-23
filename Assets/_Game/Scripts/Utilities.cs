using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities 
{
    //random thu tu mot list
    public static List<T> SortOrder<T>(List<T> list, int amount)
    {
        return list.OrderBy(d => System.Guid.NewGuid()).Take(amount).ToList();
    }

    //lay ket qua theo ty le xac suat
    public static bool Chance(int rand, int max = 100)
    {
        return UnityEngine.Random.Range(0, max) < rand;
    }
    //lay ket qua random trong khoang
    public static float RandFloatNumber(float begin, float end)
    {
        return UnityEngine.Random.Range(begin, end);
    }
    public static int RandIntNumber(int begin, int end)
    {
        return UnityEngine.Random.Range(begin, end);
    }

    //random gia enum trong mot kieu enum
    private static System.Random random = new System.Random();
    public static T RandomEnumValue<T>()
    {
        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(random.Next(v.Length));
    }

    //random gia tri tu 1 list
    public static T RandomInMember<T>(params T[] ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Length)];
    }

}

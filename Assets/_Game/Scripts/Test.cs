using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface userA
{
    public abstract string GetName();
}

public class DataA : userA
{
    public string GetName()
    {
        return "Cos";
    }
}

public class Test : MonoBehaviour
{
    userA dataA = new DataA();

    public void Start()
    {
        Debug.Log(dataA.GetName());
    }
}

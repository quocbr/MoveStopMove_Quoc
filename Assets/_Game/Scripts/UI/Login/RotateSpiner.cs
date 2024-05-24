using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpiner : MonoBehaviour
{
    [SerializeField] private float speed = -360f;
    
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * speed);
    }
}

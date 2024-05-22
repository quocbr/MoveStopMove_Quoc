using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShow : MonoBehaviour
{
    [SerializeField] Transform tf;
    [SerializeField] float angle = -6f;

    private void LateUpdate()
    {
        tf.Rotate(Vector3.up * angle, Space.Self);
    }
}

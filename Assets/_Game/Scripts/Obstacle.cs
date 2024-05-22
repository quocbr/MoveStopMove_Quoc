using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.PLAYER)
        {
            Color color = meshRenderer.material.color;
            color.a = 0.3f;
            meshRenderer.material.color = color;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layer.PLAYER)
        {
            Color color = meshRenderer.material.color;
            color.a = 1f;
            meshRenderer.material.color = color;
        }
    }
}

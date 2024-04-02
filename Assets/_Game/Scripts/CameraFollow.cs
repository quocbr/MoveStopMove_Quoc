using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Player player;
    Vector3 intialOffset = new Vector3(0, 15, -20);
    [SerializeField] Vector3 offset = new Vector3(0, 15, -20);
    float speedCamera = 0.2f;

    public Vector3 Offset { get => offset; set => offset = value; }

    private void Start()
    {
        //player == LevelManager.Ins.C_player;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.TF.position + offset, speedCamera);
        }
    }
}

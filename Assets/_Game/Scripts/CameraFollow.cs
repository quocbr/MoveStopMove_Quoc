using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{
    public enum State { MainMenu, Gameplay, Shop }


    [Header("Rotation")]
    [SerializeField] Vector3 playerRotate;
    [SerializeField] Vector3 gamePlayRotate;

    [Header("Offset")]
    [SerializeField] Vector3 playerOffset;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 offsetMax;
    [SerializeField] Vector3 offsetMin;

    [SerializeField] Transform[] offsets;

    [SerializeField] float moveSpeed = 5f;
    Transform target;

    private Vector3 targetOffset;
    private Quaternion targetRotate;

    public Camera Camera;

    private void Awake()
    {
        Camera = Camera.main;
    }
    private void Start()
    {
        if(target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
    }

    private void LateUpdate()
    {
        offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * moveSpeed);
        TF.rotation = Quaternion.Lerp(TF.rotation, targetRotate, Time.deltaTime * moveSpeed);
        TF.position = Vector3.Lerp(TF.position, target.position + targetOffset, Time.deltaTime * moveSpeed);
    }

    //rate
    public void SetRateOffset(float rate)
    {
        targetOffset = Vector3.Lerp(offsetMin, offsetMax, rate);
    }

    public void ChangeState(State state)
    {
        targetOffset = offsets[(int)state].localPosition;
        targetRotate = offsets[(int)state].localRotation;
        //return;

        switch (state)
        {
            case State.MainMenu:
                targetOffset = playerOffset;
                targetRotate = Quaternion.Euler(playerRotate);
                break;

            case State.Gameplay:
                targetOffset = offsetMin;
                targetRotate = Quaternion.Euler(gamePlayRotate);
                break;

            default:
                break;
        }
    }


    //[SerializeField] Character player;
    //Vector3 intialOffset = new Vector3(0, 15, -20);
    //[SerializeField] Vector3 offset = new Vector3(0, 15, -20);
    //[SerializeField] Vector3 zoomInOffset = new Vector3(0, 5, -10);
    //[SerializeField] Vector3 zoomInSkinShopOffset = new Vector3(0, 3, -10);
    //public float moothSpeed = 0.188f;

    //public Vector3 Offset { get => offset; set => offset = value; }
    //private void Start()
    //{
    //    player = LevelManager.Ins.Player;
    //}
    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (player != null)// Camera di theo player
    //    {
    //       Vector3 desiredPos = player.TF.position + offset;
    //       Vector3 smoothedPos = Vector3.Lerp(TF.position,desiredPos , moothSpeed);
    //       TF.position = smoothedPos;
            
    //    }
    //    //else
    //    //{
    //    //    player = LevelManager.Ins.Player;
    //    //}
    //}

    //public void SetRateOffset(float rate)
    //{
    //    //targetOffset = Vector3.Lerp(offsetMin, offsetMax, rate);
    //}

    //public void SetTargetFollow(Character target)
    //{
    //    player = target;
    //}
    //public void ResetOffset()// Tra offset ve gia tri ban dau
    //{
    //    offset = intialOffset;
    //}
    //public void ZoomOut()
    //{
    //    LevelManager.Ins.Player.transform.rotation = new Quaternion(0, 0, 0, 0);
    //    offset = intialOffset;
    //}
    //public void ZoomIn()
    //{
    //    LevelManager.Ins.Player.transform.rotation = new Quaternion(0, 180, 0, 0);
    //    if(player == null) 
    //    {
    //        player = LevelManager.Ins.Player;
    //    }
    //    Vector3 desiredPos = player.TF.position + offset;
    //    Vector3 smoothedPos = Vector3.Lerp(TF.position, desiredPos, moothSpeed);
    //    TF.position = smoothedPos;
    //    TF.LookAt(player.TF);
    //    offset = zoomInOffset + new Vector3(0, 1, -1) * player.Point;
    //}

    //public void ZoomInSkinShop()
    //{
    //    LevelManager.Ins.Player.transform.rotation = new Quaternion(0, 180, 0, 0);
    //    if (player == null)
    //    {
    //        player = LevelManager.Ins.Player;
    //    }
    //    Vector3 desiredPos = player.TF.position + offset;
    //    Vector3 smoothedPos = Vector3.Lerp(TF.position, desiredPos, moothSpeed);
    //    TF.position = smoothedPos;
    //    TF.LookAt(player.TF);
    //    offset = zoomInSkinShopOffset + new Vector3(0, 1, -1) * player.Point;
    //}
}

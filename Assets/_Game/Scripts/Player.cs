using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Player")]
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody _rb;

    private Vector3 moveVector;

    public override void OnInit()
    {
        base.OnInit();
        //TODO:
        joystick = GameManager.Ins.Joystick;
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Ins.IsState(GameState.Gameplay) == false)
        {
            return;
        }
        else
        {
            joystick = GameManager.Ins.Joystick;
        }

        Moving();

        Attack();
    }


    public override void Moving()
    {
        base.Moving();

        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            StopAllCoroutines();
            if(currentWeapon.gameObject.activeSelf == false) 
            {
                ResetAttack();
            }
            Vector3 direction = Vector3.RotateTowards(this.transform.forward, moveVector, rotationSpeed * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(direction);
            timer = 0;
            ChangeAnim(Anim.RUN);

            isAttack = false;
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            if (isAttack == false)
            {
                timer += Time.deltaTime;
                ChangeAnim(Anim.IDLE);
            }
            else
            {
                LookAtTargetDir();
               
            }
        }

        _rb.MovePosition(_rb.position + moveVector);
    }

    public void ResetEQ(UserData userData)
    {
        if(userData.currentSet != SetType.None)
        {
            ChangeSet(SaveLoadManager.Ins.UserData.currentSet);
        }
        else
        {
            ChangeHead(SaveLoadManager.Ins.UserData.currentHead);
            ChangePant(SaveLoadManager.Ins.UserData.currentPant);
            ChangeShield(SaveLoadManager.Ins.UserData.currentShield);
            ChangeTail(SaveLoadManager.Ins.UserData.currentTail);
            ChangeWing(SaveLoadManager.Ins.UserData.currentWing);
        }
    }
}

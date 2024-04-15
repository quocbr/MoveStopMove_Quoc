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
        if (characterState.Equals(CharacterState.Dead))
        {
            ChangeAnim(Anim.DEAD);
            GameManager.Ins.Lose();
            return;
        }

        if (GameManager.Ins.IsState(GameState.Gameplay) == false)
        {
            return;
        }
        else
        {
            joystick = GameManager.Ins.Joystick;
        }

        Move();

        if (targetChar != null && timer >= delayAttack)
        {
            ChangeAnim(Anim.ATTACK);
            isAttack = true;
            timer = 0;
        }
    }


    protected override void Move()
    {
        base.Move();

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

    public override void ResetEQ()
    {
        currentWeapon.poolType = SaveLoadManager.Ins.UserData.currentWeapon;

        if (SaveLoadManager.Ins.UserData.currentSet != SetType.None)
        {
            ChangeSet(SaveLoadManager.Ins.UserData.currentSet);
            return;
        }
        if (SaveLoadManager.Ins.UserData.currentHead != PoolType.None)
        {
            ChangeHead(SaveLoadManager.Ins.UserData.currentHead);
        }
        if (SaveLoadManager.Ins.UserData.currentPant != PoolType.None)
        {
            ChangePant(SaveLoadManager.Ins.UserData.currentPant);
        }
        if (SaveLoadManager.Ins.UserData.currentShield != PoolType.None)
        {
            ChangeShield(SaveLoadManager.Ins.UserData.currentShield);
        }
        if (SaveLoadManager.Ins.UserData.currentTail != PoolType.None)
        {
            ChangeTail(SaveLoadManager.Ins.UserData.currentTail);
        }
        if (SaveLoadManager.Ins.UserData.currentWing != PoolType.None)
        {
            ChangeWing(SaveLoadManager.Ins.UserData.currentWing);
        }
        

        //base.ResetEQ();
    }
}

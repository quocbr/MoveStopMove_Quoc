using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [Header("Player")]
    [SerializeField]private VariableJoystick joystick;
    [SerializeField]private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody _rb;

     private Vector3 moveVector;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void Update()
    {
        base.Update();
        Move();

        if (listTargetChar.Contains(targetChar) && timer >= delayAttack)
        {
            Attack();
            timer = 0;
        }
    }


    protected override void Move()
    {
        base.Move();

        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;

        if(joystick.Horizontal !=0 || joystick.Vertical != 0) 
        {
            StopAllCoroutines();
            Vector3 direction = Vector3.RotateTowards(this.transform.forward, moveVector, rotationSpeed * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(direction);
            timer = 0;
            ChangeAnim(Anim.RUN);
            base.isMove = true;
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            if (isAttack == true)
            {

            }
            else
            {
                timer += Time.deltaTime;
                ChangeAnim(Anim.IDLE);
                base.isMove = false;
            }
            
        }

        _rb.MovePosition(_rb.position + moveVector);
    }

}

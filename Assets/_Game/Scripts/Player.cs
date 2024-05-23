
using System;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [Header("Player")]
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool isMoving = false;
    private CounterTime counter = new CounterTime();

    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.Gameplay) || GameManager.Ins.IsState(GameState.Setting);

    private Vector3 moveVector;

    protected override void Start()
    {
        base.Start();
        joystick = GameManager.Ins.Joystick;
    }

    protected override void Update()
    {
        base.Update();

        if (IsCanUpdate && !IsDead)
        {
            joystick = GameManager.Ins.Joystick;
            Moving();
        }

        //if(IsDead) return;

        //if (GameManager.Ins.IsState(GameState.Gameplay) == false)
        //{
        //    return;
        //}
        //else
        //{
        //    joystick = GameManager.Ins.Joystick;
        //}

        //Moving();

        //Attack();
    }

    protected override void SetSize(float size)
    {
        base.SetSize(size);
        CameraFollower.Ins.SetRateOffset((this.size - MIN_SIZE) / (MAX_SIZE - MIN_SIZE));
    }

    public override void OnInit()
    {
        base.OnInit();

        TF.rotation = Quaternion.Euler(Vector3.up * 180);
        SetSize(MIN_SIZE);

        indicator.SetName(NameChar);

        
    }

    public override void OnAttack()
    {
        base.OnAttack();
        if (TargetChar != null && (CurrentWeapon as Weapon).IsCanAttack)
        {
            counter.Start(Throw, TIME_DELAY_THROW);
            ResetAnim();
        }
    }

    public override void AddTarget(Character target)
    {
        base.AddTarget(target);

        if (!target.IsDead && !IsDead)
        {
            if (!counter.IsRunning && !isMoving)
            {
                OnAttack();
            }
        }
    }

    public override void SetPoint(int point)
    {
        base.SetPoint(point);
        if (this.Point % 3 == 0)
        {
            _particleSystem.Play();
            SoundManager.Ins.PlaySFX(Constant.SFXSound.SIZE_UP);
        }
    }

    public override void WearClothes()
    {
        base.WearClothes();
        ColorSkin.material = SpawnManager.Ins.playerColor;
        SaveLoadManager.Ins.UserData.currentColor = ColorSkin.material;
        ChangeWeapon(EquipmentController.Ins.GetWeapon());

        ChangeColorSkin(SaveLoadManager.Ins.UserData.currentColor);

        ChangeWeapon(SaveLoadManager.Ins.UserData.CurrentWeapon);

        if (SaveLoadManager.Ins.UserData.CurrentSet != SetType.None)
        {
            ChangeSet(SaveLoadManager.Ins.UserData.CurrentSet);
        }
        else
        {
            ChangeHead(SaveLoadManager.Ins.UserData.CurrentHead);
            ChangePant(SaveLoadManager.Ins.UserData.CurrentPant);
            ChangeTail(SaveLoadManager.Ins.UserData.CurrentTail);
            ChangeShield(SaveLoadManager.Ins.UserData.CurrentShield);
            ChangeWing(SaveLoadManager.Ins.UserData.CurrentWing);
        }

        Buff.HandleBuff(this);
    }
    public override void Moving()
    {
        //base.Moving();

        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * (moveSpeed + moveSpeed*Buff.buffMoveSpeed/100f) * Time.deltaTime;
        moveVector.z = joystick.Vertical * (moveSpeed + moveSpeed* Buff.buffMoveSpeed / 100f) * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            counter.Cancel();
        }

        if (Input.GetMouseButton(0) && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            //StopAllCoroutines();
            //if(CurrentWeapon.gameObject.activeSelf == false) 
            //{
            //    ResetAttack();
            //}
            Vector3 direction = Vector3.RotateTowards(this.transform.forward, moveVector, rotationSpeed * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(direction);
            timer = 0;
            ChangeAnim(Anim.RUN);
            characterState = CharacterState.Run;

            isAttack = false;
            //them
            isMoving = true;

            _rb.MovePosition(_rb.position + moveVector);
            TF.position = _rb.position;
        }
        else
        {
            counter.Execute();

        }
        //else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        //{
        //    if (isAttack == false)
        //    {
        //        timer += Time.deltaTime;
        //        //ChangeAnim(Anim.IDLE);
        //        StopMoving();
        //    }
        //    else
        //    {
        //        LookAtTargetDir();

        //    }
        //}

        if (Input.GetMouseButtonUp(0) && isMoving)
        {
            isMoving = false;
            StopMoving();
            OnAttack();
        }

        //if (Input.GetMouseButtonUp(0) && characterState != CharacterState.Idle)
        //{
        //    isMoving = false;
        //    StopMoving();
        //}

        //if(isAttack == true)
        //{
        //    LookAtTargetDir();
        //}
    }

    public override void StopMoving()
    {
        base.StopMoving();
        _rb.velocity = Vector3.zero;
        characterState = CharacterState.Idle;
        ChangeAnim(Anim.IDLE);
    }

    public override void ResetEQ(UserData userData)
    {
        if (userData.currentSet != SetType.None)
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
            ChangeColorSkin(SaveLoadManager.Ins.UserData.currentColor);
        }
        Buff.HandleBuff(this);

    }

    public override void ChangeWeapon(PoolType weaponType)
    {
        EquipmentData data;
        if (CurrentWeapon != null)
        {
            data = EquipmentController.Ins.GetWeapon(CurrentWeapon.poolType);
            Buff.HandleAddBuff(data.buff, -data.value);
        }

        base.ChangeWeapon(weaponType);
        data = EquipmentController.Ins.GetWeapon(weaponType);
        Buff.HandleAddBuff(data.buff, data.value);
    }

    internal void OnRevive()
    {
        ChangeAnim(Anim.IDLE);
        IsDead = false;
        listTargetChar.Clear();
        TargetChar = null;
        //reviveVFX.Play();
    }
}

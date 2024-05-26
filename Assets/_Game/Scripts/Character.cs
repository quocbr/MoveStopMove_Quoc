using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public enum CharacterState { Idle, Run, Attack, Dead, Win };

public class Character : GameUnit
{

    [Header("Charracter")]
    public const float TIME_DELAY_THROW = 0.4f;

    public const float ATT_RANGE = 5f;

    public const float MAX_SIZE = 4f;
    public const float MIN_SIZE = 1f;

    [SerializeField] private Animator anim;

    [SerializeField] private ColorType currentColor;
    [SerializeField] private Transform throwPos;
    [SerializeField] private SkinnedMeshRenderer colorSkin;
    [SerializeField] private Transform headPos;
    [SerializeField] private Transform pantPos;
    [SerializeField] private Transform wingPos;
    [SerializeField] private Transform tailPos;
    [SerializeField] private Transform shieldPos;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private SkinnedMeshRenderer pantType;
    [SerializeField]protected ParticleSystem buffGiftParticleSystem;
    

    [SerializeField] private AudioSource sFXSource;

    [SerializeField] private AnimationEvent animEvent;
    //[SerializeField] private float maxScale = 2.5f;

    [SerializeField] Transform indicatorPoint;
    protected TargetIndicator indicator;

    [SerializeField] private AttributeBuff buff;

    private int point = 0;
    private string currentAnimName;
    private Vector3 attackDir;
    private string nameChar;
    private string killByName = null;
    protected float size = 1;

    protected CharacterState characterState;
    protected List<Character> listTargetChar = new List<Character>();
    private Character targetChar;
    protected Vector3 targetPoint;

    protected float timer = 0;


    private GameUnit currentHead;
    private PoolType currentPant;
    private GameUnit currentWing;
    private GameUnit currentTail;
    private GameUnit currentShield;
    private SetType currentSet;

    private GameUnit currentWeapon;

    //Effec
    

    protected bool isAttack = false;
    protected bool isAttacking = false;

    protected bool isDead;
    protected bool isMove;

    public bool EatedGiftBox = false;

    protected IState<Character> currentState;

    public bool IsDead { get => isDead; set => isDead = value; }
    public int Point { get => point; set => point = value; }
    public string NameChar { get => nameChar; set => nameChar = value; }
    public ColorType CurrentColor { get => currentColor; set => currentColor = value; }
    public Vector3 AttackDir { get => attackDir; set => attackDir = value; }
    public Transform ThrowPos { get => throwPos; set => throwPos = value; }
    public SkinnedMeshRenderer ColorSkin { get => colorSkin; set => colorSkin = value; }
    public string KillByName { get => killByName; set => killByName = value; }
    public GameUnit CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }
    public bool IsCanAttack => (currentWeapon as Weapon).IsCanAttack;
    public float Size { get => size; set => size = value; }
    public Character TargetChar { get => targetChar; set => targetChar = value; }
    public Vector3 TargetPoint { get => targetPoint; }
    public AudioSource SFXSource { get => sFXSource; set => sFXSource = value; }
    public GameUnit CurrentHead { get => currentHead; set => currentHead = value; }
    public PoolType CurrentPant { get => currentPant; set => currentPant = value; }
    public GameUnit CurrentWing { get => currentWing; set => currentWing = value; }
    public GameUnit CurrentTail { get => currentTail; set => currentTail = value; }
    public GameUnit CurrentShield { get => currentShield; set => currentShield = value; }
    public SetType CurrentSet { get => currentSet; set => currentSet = value; }
    public AttributeBuff Buff { get => buff; set => buff = value; }

    protected virtual void Start()
    {
        //OnInit();
    }

    protected virtual void Update()
    {

    }

    protected virtual void LookAtTargetDir()
    {
        if (TargetChar != null)
        {
            this.transform.LookAt(TargetChar.TF);
            AttackDir = TargetChar.TF.position - ThrowPos.position;
        }

    }
    // Logic when bullet hit victim
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        AddPoint(1);
        victim.DoDead(attacker.NameChar);
        attacker.RemoveTarget(victim);
    }
    protected virtual void AddPoint(int point = 1)
    {
        SetPoint(this.Point + point);
        //this.Point += point > 0 ? point : 1;
        //if (TF.localScale.x > maxScale) return;
        //TF.localScale = Vector3.one + Vector3.one * (this.Point / 3) * 0.1f;
    }
    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, MIN_SIZE, MAX_SIZE);
        this.Size = size;
        TF.localScale = size * Vector3.one;
    }

    public virtual void SetPoint(int point)
    {
        this.point = point > 0 ? point : 0;
        indicator.SetScore(this.point);
        SetSize(1 + this.point * 0.1f);
    }

    public virtual void OnInit()
    {
        listTargetChar.Clear();
        //currentAnimName = Anim.IDLE;
        //TF.localScale = Vector3.one;
        //isAttack = false;
        //isAttacking = false;
        SFXSource.clip = null;
        isDead = false;
        point = 0;
        //timer = 0;
        TargetChar = null;
        killByName = null;
        characterState = CharacterState.Idle;

        WearClothes();

        indicator = HBPool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        indicator.SetTarget(indicatorPoint);
        indicator.SetColor(SpawnManager.Ins.GetColorSkin(CurrentColor).color);
    }

    public virtual void WearClothes()
    {

    }

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void ResetAnim()
    {
        currentAnimName = "";
    }

    #region Moving
    public virtual void Moving()
    {
        characterState = CharacterState.Run;
    }

    public virtual void StopMoving()
    {
        characterState = CharacterState.Idle;
    }
    #endregion

    #region Attack
    public virtual void OnAttack()
    {
        TargetChar = GetTargetInRange();

        if (IsCanAttack && TargetChar != null && !TargetChar.IsDead/* && currentSkin.Weapon.IsCanAttack*/)
        {
            targetPoint = TargetChar.TF.position;
            targetPoint.y = TF.transform.position.y;
            if (this is Player)
            {
                (TargetChar as Bot).SetMark(true);
            }

            //TF.LookAt(TargetChar.TF.position + (TF.position.y - TargetChar.TF.position.y) * Vector3.up);
            TF.LookAt(targetPoint);
            ChangeAnim(Anim.ATTACK);
        }

    }
    //public virtual bool CheckAttack()
    //{
    //    return listTargetChar.Count != 0;
    //}
    //public virtual void Attack()
    //{
    //    //if (targetChar != null && timer >= delayAttack)
    //    //{
    //    //    ChangeAnim(Anim.ATTACK);
    //    //    isAttack = true;
    //    //    timer = 0;
    //    //}

    //    if (TargetChar != null && currentWeapon.gameObject.activeSelf == true)
    //    {
    //        ChangeAnim(Anim.ATTACK);
    //        isAttack = true;
    //        timer = 0;
    //    }
    //}

    //public virtual void Attacking()
    //{
    //    if (timer > TIME_DELAY_THROW)
    //    {
    //        Throw();
    //        timer = 0;
    //    }
    //    //isAttack = true;
    //    //LookAtTargetDir();
    //    //Throw();
    //    //CurrentWeapon.gameObject.SetActive(false);
    //}

    //public void ResetAttack()
    //{
    //    isAttack = false;
    //    CurrentWeapon.gameObject.SetActive(true);
    //}

    public virtual void Throw()
    {
        PlaySFX(Constant.SFXSound.WEAPON_THROW);
        (CurrentWeapon as Weapon).Throw(this, OnHitVictim);
        if(EatedGiftBox == true)
        {
            HandleEndEatGiftBox();
        }
    }

    public Character GetTargetInRange()
    {
        Character target = null;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < listTargetChar.Count; i++)
        {
            if (listTargetChar[i] != null && listTargetChar[i] != this && !listTargetChar[i].IsDead && Vector3.Distance(TF.position, listTargetChar[i].TF.position) <= ATT_RANGE * size + listTargetChar[i].size)
            {
                float dis = Vector3.Distance(TF.position, listTargetChar[i].TF.position);

                if (dis < distance)
                {
                    distance = dis;
                    target = listTargetChar[i];
                }
            }
        }
        return target;
    }
    #endregion Attack

    #region Target
    public virtual void AddTarget(Character target)
    {
        //if (listTargetChar.Count == 0)
        //{
        //    targetChar = target;
        //    if (this is Player)
        //    {
        //        (targetChar as Bot).SetActiveTargetImage(true);
        //    }
        //}
        this.listTargetChar.Add(target);
    }

    public virtual void RemoveTarget(Character target)
    {
        if (this is Player)
        {
            (target as Bot).SetMark(false);
        }
        this.listTargetChar.Remove(target);
        if (listTargetChar.Count == 0)
        {
            TargetChar = null;
        }
        else
        {
            TargetChar = listTargetChar[Random.Range(0, listTargetChar.Count)];
            if (this is Player)
            {
                (TargetChar as Bot).SetMark(true);
            }
        }
    }

    #endregion

    #region Hanlde Dead
    public virtual void DoDead(string name = null)
    {
        if (!IsDead)
        {
            IsDead = true;
            characterState = CharacterState.Dead;
            PlaySFX(Constant.SFXSound.CHARACTER_DIE);
            ChangeAnim(Anim.DEAD);
            killByName = name;
            HBPool.Despawn(indicator);
            LevelManager.Ins.HandleCharecterDeath(this);
        }
        //if (this is Player)
        //{
        //    GameManager.Ins.Lose();
        //}
    }

    public virtual void OnDespawn()
    {
        //tra ve tat ca nhung object pool
        RemoveAllEQ();
    }

    #endregion



    #region Change Eq
    public virtual void ChangeColorSkin(Material material)
    {
        colorSkin.material = material;
    }

    public virtual void ChangeWeapon(PoolType weaponType)
    {
        if (CurrentWeapon != null)
        {
            //Destroy(currentWeapon.gameObject);
            HBPool.Despawn(CurrentWeapon);
            CurrentWeapon = null;
        }
        //currentWeapon = Instantiate(weapon);
        CurrentWeapon = HBPool.Spawn<GameUnit>(weaponType);
        CurrentWeapon.transform.SetParent(weaponPos, false);
        CurrentWeapon.TF.localPosition = Vector3.zero;
        CurrentWeapon.TF.localRotation = Quaternion.identity;
        CurrentWeapon.TF.localScale = Vector3.one;
    }

    public virtual void ChangeHead(PoolType headType, bool isTry = false)
    {
        if (CurrentHead != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(CurrentHead);
            CurrentHead = null;
        }

        if (headType == PoolType.None) return;
        CurrentHead = HBPool.Spawn<GameUnit>(headType);
        CurrentHead.transform.SetParent(headPos, false);
        CurrentHead.TF.localPosition = Vector3.zero;
        CurrentHead.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangePant(Material material, bool isTry = false)
    {
        if (material == null)
        {
            pantType.enabled = false;
        }
        else
        {
            pantType.enabled = true;
            pantType.material = material;
        }
    }

    public virtual void ChangePant(PoolType poolType, bool isTry = false)
    {
        Material material = EquipmentController.Ins.GetPant(poolType);
        if (material == null)
        {

            pantType.enabled = false;
            CurrentPant = PoolType.None;
        }
        else
        {
            pantType.enabled = true;
            pantType.material = material;
            CurrentPant = poolType;
        }
    }

    public virtual void ChangeWing(PoolType wingType)
    {
        if (CurrentWing != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(CurrentWing);
            CurrentWing = null;
        }

        if (wingType == PoolType.None) return;

        CurrentWing = HBPool.Spawn<GameUnit>(wingType);
        CurrentWing.transform.SetParent(wingPos, false);
        CurrentWing.TF.localPosition = Vector3.zero;
        CurrentWing.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeTail(PoolType tailType)
    {
        if (CurrentTail != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(CurrentTail);
            CurrentTail = null;
        }

        if (tailType == PoolType.None) return;

        CurrentTail = HBPool.Spawn<GameUnit>(tailType);
        CurrentTail.transform.SetParent(wingPos, false);
        CurrentTail.TF.localPosition = Vector3.zero;
        CurrentTail.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeShield(PoolType shieldType, bool isTry = false)
    {
        if (CurrentShield != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(CurrentShield);
            CurrentShield = null;
        }

        if (shieldType == PoolType.None) return;
        CurrentShield = HBPool.Spawn<GameUnit>(shieldType);
        CurrentShield.transform.SetParent(shieldPos, false);
        CurrentShield.TF.localPosition = Vector3.zero;
        CurrentShield.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeSet(SetType setType, bool isTry = false)
    {
        if (setType == SetType.None)
        {
            if (CurrentSet != SetType.None)
            {
                RemoveAllEQ();
                
            }
            CurrentSet = setType;
            return;
        }

        ChangeHead(EquipmentController.Ins.GetHead(setType));
        ChangePant(EquipmentController.Ins.GetPant(setType));
        ChangeWing(EquipmentController.Ins.GetWing(setType));
        ChangeTail(EquipmentController.Ins.GetTail(setType));
        ChangeShield(EquipmentController.Ins.GetShield(setType));

        ChangeColorSkin(SpawnManager.Ins.GetColorSkinSet(setType));

        CurrentSet = setType;
    }

    public virtual void RemoveAllEQ()
    {
        ChangeHead(PoolType.None);
        ChangePant(null);
        ChangeWing(PoolType.None);
        ChangeTail(PoolType.None);
        ChangeShield(PoolType.None);
    }

    public virtual void ResetEQ(UserData userData)
    {
        //if (userData.currentSet != SetType.None)
        //{
        //    ChangeSet(SaveLoadManager.Ins.UserData.currentSet);
        //}
        //else
        //{
        //    ChangeHead(SaveLoadManager.Ins.UserData.currentHead);
        //    ChangePant(SaveLoadManager.Ins.UserData.currentPant);
        //    ChangeShield(SaveLoadManager.Ins.UserData.currentShield);
        //    ChangeTail(SaveLoadManager.Ins.UserData.currentTail);
        //    ChangeWing(SaveLoadManager.Ins.UserData.currentWing);
        //    ChangeColorSkin(SaveLoadManager.Ins.UserData.currentColor);
        //}
    }


   

    #endregion

    public virtual void SetNameChar(string name)
    {
        nameChar = name;
        indicator.SetName(name);
    }

    protected virtual void PlaySFX(string nameSFX)
    {
        if (SoundManager.Ins.isMute == true) return;
        try
        {
            SFXSource.clip = SoundManager.Ins.GetSFX(nameSFX);
            SFXSource.Play();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Character Sound ERROR: " + e);
        }
    }

    public virtual void HandleEatGiftBox()
    {
        EatedGiftBox = true;
        buffGiftParticleSystem.Play();
        TF.localScale += Vector3.one*0.5f;
    }

    protected virtual void HandleEndEatGiftBox()
    {
        TF.localScale -= Vector3.one * 0.5f;
        buffGiftParticleSystem.Stop();
        EatedGiftBox = false;
    }

    public void ChangeState(IState<Character> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}

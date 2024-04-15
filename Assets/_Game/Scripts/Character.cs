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

    [SerializeField] private AnimationEvent animEvent;

    private int point = 0;
    private string currentAnimName;
    private Vector3 attackDir;
    private string nameChar;
    private string killByName = null;

    protected CharacterState characterState;
    protected List<Character> listTargetChar;
    protected Character targetChar;
    protected float delayAttack = 1f;
    protected float timer = 0;

    
    protected GameUnit currentHead;
    protected PoolType currentPant;
    protected GameUnit currentWing;
    protected GameUnit currentTail;
    protected GameUnit currentShield;
    protected GameUnit currentWeapon;
    protected SetType currentSet;
    
    protected bool isAttack = false;
    protected bool isAttacking = false;

    protected bool isDead;
    protected bool isMove;

    protected IState<Character> currentState;

    public bool IsDead { get => isDead; set => isDead = value; }
    public int Point { get => point; set => point = value; }
    public string NameChar { get => nameChar; set => nameChar = value; }
    public ColorType CurrentColor { get => currentColor; set => currentColor = value; }
    public Vector3 AttackDir { get => attackDir; set => attackDir = value; }
    public Transform ThrowPos { get => throwPos; set => throwPos = value; }
    public SkinnedMeshRenderer ColorSkin { get => colorSkin; set => colorSkin = value; }
    public string KillByName { get => killByName; set => killByName = value; }

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void Update()
    {
        if(GameManager.Ins.IsState(GameState.Gameplay) == false)
        {
            return;
        }
    }

    public virtual void OnInit()
    {
        listTargetChar = new List<Character>();
        IsDead = false;
        currentAnimName = Anim.IDLE;
        TF.localScale = Vector3.one;
        isAttack = false;
        isAttacking = false;
        isDead = false;
        point = 0;
        timer = 0;
        targetChar = null;
        listTargetChar.Clear();
        killByName = null;
        characterState = CharacterState.Idle;
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

    protected virtual void LookAtTargetDir()
    {
        if(targetChar != null)
        {
            this.transform.LookAt(targetChar.TF);
            AttackDir = targetChar.TF.position - ThrowPos.position;
        }
        
    }

    protected virtual void Move()
    {

    }

#region Attack
    public virtual void Attack()
    {
        
        isAttack = true;
        LookAtTargetDir();
        Throw();
        currentWeapon.gameObject.SetActive(false);
    }

    public void ResetAttack()
    {
        isAttack = false;
        currentWeapon.gameObject.SetActive(true);
    }

    public void Throw()
    {
        (currentWeapon as Weapon).Throw(this, OnHitVictim);
    }
    // Logic when bullet hit victim
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        AddPoint(victim.Point);
        victim.DoDead(attacker.NameChar);
        attacker.RemoveTarget(victim);
    }

    #endregion Attack

    

    #region Target
    public virtual void AddTarget(Character target)
    {
        if (listTargetChar.Count == 0)
        {
            targetChar = target;
            if (this is Player)
            {
                (targetChar as Bot).SetActiveTargetImage(true);
            }
        }
        this.listTargetChar.Add(target);
    }

    public virtual void RemoveTarget(Character target)
    {
        if (this is Player)
        {
                (target as Bot).SetActiveTargetImage(false);
        }
        this.listTargetChar.Remove(target);
        if (listTargetChar.Count == 0)
        {
            targetChar = null;
        }
        else
        {
            targetChar = listTargetChar[Random.Range(0, listTargetChar.Count)];
            if (this is Player)
            {
                (targetChar as Bot).SetActiveTargetImage(true);
            }
        }
    }

#endregion

#region Hanlde Dead
    public virtual void DoDead(string name = null)
    {
        IsDead = true;
        characterState = CharacterState.Dead;
        killByName = name;
        //TODO:Xu ly khi    
        ChangeAnim(Anim.DEAD);
        LevelManager.Ins.HandleCharacterDead(this);
    }

#endregion

    protected virtual void AddPoint(int point)
    {
        this.Point += point > 0 ? point : 1;
        TF.localScale = Vector3.one + Vector3.one * this.Point * 0.2f;
    }

#region Change Eq
    public virtual void ChangeColorSkin(Material material)
    {
        colorSkin.material = material; 
    }

    public virtual void ChangeWeapon(PoolType weaponType)
    {
        if (currentWeapon != null)
        {
            //Destroy(currentWeapon.gameObject);
            HBPool.Despawn(currentWeapon);
            currentWeapon = null;
        }
        //currentWeapon = Instantiate(weapon);
        currentWeapon = HBPool.Spawn<GameUnit>(weaponType);
        currentWeapon.transform.SetParent(weaponPos,false);
        currentWeapon.TF.localPosition = Vector3.zero;
        currentWeapon.TF.localRotation = Quaternion.identity;
        currentWeapon.TF.localScale = Vector3.one;
    }

    public virtual void ChangeHead(PoolType headType)
    {
        if(currentHead != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(currentHead);
            currentHead = null;
        }

        if (headType == PoolType.None) return;

        currentHead = HBPool.Spawn<GameUnit>(headType);
        currentHead.transform.SetParent(headPos, false);
        currentHead.TF.localPosition = Vector3.zero;
        currentHead.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangePant(Material material)
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
    
    public virtual void ChangePant(PoolType poolType)
    {
        Material material = EquipmentController.Ins.GetPant(poolType);
        if (material == null)
        {
            pantType.enabled = false;
            currentPant = PoolType.None;
        }
        else
        {
            pantType.enabled = true;
            pantType.material = material;
            currentPant = poolType;
        }
    }

    public virtual void ChangeWing(PoolType wingType)
    {
        if (currentWing != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(currentWing);
            currentWing = null;
        }

        if (wingType == PoolType.None) return;

        currentWing = HBPool.Spawn<GameUnit>(wingType);
        currentWing.transform.SetParent(wingPos, false);
        currentWing.TF.localPosition = Vector3.zero;
        currentWing.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeTail(PoolType tailType)
    {
        if (currentTail != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(currentTail);
            currentTail = null;
        }

        if (tailType == PoolType.None) return;

        currentTail = HBPool.Spawn<GameUnit>(tailType);
        currentTail.transform.SetParent(wingPos, false);
        currentTail.TF.localPosition = Vector3.zero;
        currentTail.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeShield(PoolType shieldType)
    {
        if (currentShield != null)
        {
            //Destroy(currentHead.gameObject);
            HBPool.Despawn(currentShield);
            currentShield = null;
        }

        if (shieldType == PoolType.None) return;

        currentShield = HBPool.Spawn<GameUnit>(shieldType);
        currentShield.transform.SetParent(shieldPos, false);
        currentShield.TF.localPosition = Vector3.zero;
        currentShield.TF.localRotation = Quaternion.identity;
    }

    public virtual void ChangeSet(SetType setType)
    {
        ChangeHead(EquipmentController.Ins.GetHead(setType));
        ChangePant(EquipmentController.Ins.GetPant(setType));
        ChangeWing(EquipmentController.Ins.GetWing(setType));
        ChangeTail(EquipmentController.Ins.GetTail(setType));
        ChangeShield(EquipmentController.Ins.GetShield(setType));

        ChangeColorSkin(SpawnManager.Ins.GetColorSkinSet(setType));

        currentSet = setType;
    }

    public virtual void ResetEQ()
    {
        ChangeWeapon(currentWeapon.poolType);
        if(currentSet != SetType.None)
        {
            ChangeSet(currentSet);
        }
        else
        {
            ChangeHead(currentHead.poolType);
            ChangePant(currentPant);
            ChangeWing(currentWing.poolType);
            ChangeTail(currentTail.poolType);
            ChangeShield(currentShield.poolType);
        }
    }

    public virtual void RemoveAllEQ()
    {
        ChangeHead(PoolType.None);
        ChangePant(null);
        ChangeWing(PoolType.None);
        ChangeTail(PoolType.None);
        ChangeShield(PoolType.None);
    }

    #endregion

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

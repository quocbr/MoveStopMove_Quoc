using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class Character : GameUnit
{
    [Header("Charracter")]
    [SerializeField] private Animator anim;
    [SerializeField] private Weapon currentSkin;

    [SerializeField] protected GameObject weapon;

    private string currentAnimName;

    protected List<Character> listTargetChar;
    protected Character targetChar;
    protected float delayAttack = 1f;
    protected float timer = 0;
    private int point = 0;

    protected bool isAttack = false;
    protected bool isAttacking = false;

    protected bool isDead;
    protected bool isMove;

    public Vector3 attackDir;
    public Transform throwPos;

    public bool IsDead { get => isDead; set => isDead = value; }
    public int Point { get => point; set => point = value; }
    private void OnDisable()
    {
        TF.localScale = Vector3.one;
        isAttack = false;
        isAttacking = false;
        isDead = false;
        point = 0;
        timer = 0;
        targetChar = null;
        listTargetChar.Clear();
        ChangeAnim(Anim.IDLE);
    }

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnInit()
    {
        listTargetChar = new List<Character>();
        IsDead = false;
    }

    protected virtual void ChangeAnim(string animName)
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
        this.transform.LookAt(targetChar.TF);
    }

    protected virtual void Move()
    {

    }

    public virtual void Attack()
    {
        LookAtTargetDir();
        ChangeAnim(Anim.ATTACK);
        isAttack = true;
        attackDir = targetChar.TF.position - throwPos.position;
        StartCoroutine(IE_Throw());
    }

    IEnumerator IE_Throw()
    {
        yield return new WaitForSeconds(0.1f);
        isAttacking = true;
        weapon.SetActive(false);
        Throw();
        StartCoroutine(nameof(IE_DeAttack));
    }

    IEnumerator IE_DeAttack()
    {
        yield return new WaitForSeconds(0.6f);
        weapon.SetActive(true);
        isAttack = false;
        isAttacking = false;
    }

    protected virtual void ResetAttack()
    {
        isAttack = false;
        isAttacking = false;
        weapon.SetActive(true);
    }


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

    public virtual void RemoveCharacter(Character character)
    {
        if (listTargetChar.Contains(character))
        {
            listTargetChar.Remove(character);
        }
    }

    public virtual void DoDead()
    {


        IsDead = true;
        //TODO:Xu ly khi dead
        StartCoroutine(nameof(HandleDead));

    }

    protected virtual IEnumerator HandleDead()
    {
        ChangeAnim(Anim.DEAD);
        yield return new WaitForSeconds(2f);
        LevelManager.Ins.RemoveCharacter(this);
        HBPool.Despawn(this);
    }


    public void Throw()
    {
        currentSkin.Throw(this, OnHitVictim);
    }
    // Logic when bullet hit victim
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        AddPoint(victim.Point);
        victim.DoDead();
        attacker.RemoveTarget(victim);
    }

    protected virtual void AddPoint(int point)
    {
        this.Point += point > 0 ? point : 1;
        TF.localScale = Vector3.one + Vector3.one * this.Point * 0.1f;
    }

}

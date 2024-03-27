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
    
    
    private string currentAnimName;
    protected List<Character> listTargetChar;
    protected Character targetChar;
    protected float delayAttack = 2f;
    protected float timer = 0;

    protected bool isAttack = false;
    protected bool isAttacking = false;

    protected bool isDead;
    protected bool isMove;
    
    public Transform throwPos;
    
    public bool IsDead { get => isDead; set => isDead = value; }

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void Update()
    {
        if(isAttacking == true)
        {
            return;
        }
    }

    protected virtual void OnInit()
    {
        listTargetChar = new List<Character>();
        isMove = false;
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

    protected virtual void SetTargetDir()
    {
        //this.transform.LookAt(targetChar.TF);
    }

    protected virtual void Move()
    {
        
    }

    protected virtual void Attack()
    {
        if (isAttack)
        {
            return;
        }
        SetTargetDir();
        ChangeAnim(Anim.ATTACK);
        isAttack = true;
        Vector3 direct = targetChar.TF.position - throwPos.position;
        float max = Mathf.Max(Mathf.Abs(direct.x), Mathf.Abs(direct.y), Mathf.Abs(direct.z));
        direct /= max;
        StartCoroutine(IE_Throw(direct));
    }

    IEnumerator IE_Throw(Vector3 dir)
    {
        yield return new WaitForSeconds(0.1f);
        Throw();
        isAttacking = true;
        yield return new WaitForSeconds(0.88f);
        isAttack = false;
        isAttacking = false;
    }

    public virtual void AddTarget(Character target) 
    {
        if(listTargetChar.Count == 0)
        {
            targetChar = target;
        }
        this.listTargetChar.Add(target);
    }

    public virtual void RemoveTarget(Character target)
    {
        this.listTargetChar.Remove(target);
        if(listTargetChar.Count == 0)
        {
            targetChar = null;
        }
        else
        {
            targetChar = listTargetChar[Random.Range(0, listTargetChar.Count)];
        }
    }

    public void DoDead()
    {

    }

    

    public void Throw()
    {
        Debug.Log("Co");
        currentSkin.Throw(this, OnHitVictim);
    }
    // Logic when bullet hit victim
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        victim.DoDead();

    }

}

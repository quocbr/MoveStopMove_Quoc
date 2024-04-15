using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject targetImage;
    

    public float range;
    public Transform centrePoint;

    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        base .Update();
        if (IsDead == true)
        {
            ChangeState(new DeadState());
            return;
        }


        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        SetActiveTargetImage(false);
    }

    public void StopMoving()
    {
        ChangeAnim(Anim.IDLE);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public void Moving()
    {
        ChangeAnim(Anim.RUN);
        Patrol();
        agent.isStopped = false;
    }

    public bool IsFinish()
    {
        return agent.remainingDistance <= 0.1f;
    }

    public void Patrol()
    {   
        while (true)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
                break;
            }
                
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    public bool CheckAttack()
    {
        return listTargetChar.Count != 0;
    }

    public void BotAttack()
    {
        
        if (isAttack == false)
        {
            timer += Time.deltaTime;
            ChangeState(new IdleState());
        }
        else
        {
            LookAtTargetDir();
        }

        if(listTargetChar.Contains(targetChar) && timer >= delayAttack)
        {
            ChangeAnim(Anim.ATTACK);
            isAttack = true;
            timer = 0;
        }
    }

    //TODO:
    public void SetActiveTargetImage(bool r)
    {
        targetImage.SetActive(r);
    }

    public void HandleBeginDead()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public void HandleEndDead()
    {
        if (LevelManager.Ins.RemainBot < 1)
        {
            HBPool.Despawn(this);
        }
        else
        {
            LevelManager.Ins.RemainBot -= 1;
            UIManager.Ins.GetUI<GamePlay>().SetAliveText(LevelManager.Ins.RemainBot);
            OnInit();
            ResetEQ1();
            Vector3 centre = LevelManager.Ins.GetRandomSpawnPos();
            while (true)
            {
                Vector3 point;
                if (RandomPoint(centre, 8f, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    ChangeState(new IdleState());
                    TF.position = point;
                    break;
                }

            }
        }
    }

    public override void DoDead(string name)
    {
        this.SetActiveTargetImage(false);
        base.DoDead();
    }

    private void ResetEQ1()
    {
        int randomeq = Random.Range(1, 101);

        if (randomeq <= 3)
        {
            ChangeSet(EquipmentController.Ins.GetSet());
        }
        else{ 
            if (Random.Range(1, 101) > 50) ChangeHead(EquipmentController.Ins.GetHead());
            if (Random.Range(1, 101) > 40) ChangePant(EquipmentController.Ins.GetPant());
            if (Random.Range(1, 101) > 95) ChangeTail(EquipmentController.Ins.GetTail());
            if (Random.Range(1, 101) > 70) ChangeShield(EquipmentController.Ins.GetShield());
            if (Random.Range(1, 101) > 95) ChangeWing(EquipmentController.Ins.GetWing()); 
        }
        ChangeWeapon(EquipmentController.Ins.GetWeapon());
    }
}

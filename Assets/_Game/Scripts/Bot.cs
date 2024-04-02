using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject targetImage;
    private IState<Character> currentState;

    public float range;
    public Transform centrePoint;

    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsDead == true)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            ChangeAnim(Anim.DEAD);
            return;
        }

        base .Update();
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
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

    public void StopMoving()
    {
        ChangeAnim("idle");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public void Moving()
    {
        ChangeAnim("run");
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
        return listTargetChar.Count !=0;
    }

    public void BotAttack()
    {
        
        if (isAttack == false)
        {
            timer += Time.deltaTime;
            ChangeAnim(Anim.IDLE);
        }

        if(listTargetChar.Contains(targetChar) && timer >= delayAttack)
        {
            Attack();
            timer = 0;
        }
    }

    //TODO:
    public void SetActiveTargetImage(bool r)
    {
        targetImage.SetActive(r);
    }

    public override void DoDead()
    {
        this.SetActiveTargetImage(false);
        base.DoDead();
        
    }
}

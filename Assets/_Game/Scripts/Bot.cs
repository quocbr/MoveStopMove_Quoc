using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject mask;

    private CounterTime counter = new CounterTime();
    public CounterTime Counter => counter;

    private Vector3 destination;


    public float range;
    public Transform centrePoint;

    private bool IsCanRunning => (GameManager.Ins.IsState(GameState.Gameplay) || GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting));

    protected override void Update()
    {
        if (IsCanRunning && currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        SetMark(false);
        ResetAnim();

        NameChar = NameUtilities.GetRandomName();
        indicator.SetName(NameChar);
        //indicator.SetColor(SpawnManager.Ins.GetColorSkin(CurrentColor).color);
    }

    public override void WearClothes()
    {
        base.WearClothes();
        //Tuple<Material, int> color = SpawnManager.Ins.GetColor();
        //ChangeColorSkin(color.Item1);
        //this.CurrentColor = (ColorType)color.Item2;
        //ColorSkin.material = SpawnManager.Ins.GetColor();
        ChangeWeapon(EquipmentController.Ins.GetWeapon());

        if (Utilities.Chance(3))
        {
            ChangeSet(EquipmentController.Ins.GetSet());
        }
        else
        {
            if (Utilities.Chance(80))ChangeHead(EquipmentController.Ins.GetHead());
            if (Utilities.Chance(60)) ChangePant(EquipmentController.Ins.GetPant());
            if (Utilities.Chance(5)) ChangeTail(EquipmentController.Ins.GetTail());
            if (Utilities.Chance(10)) ChangeShield(EquipmentController.Ins.GetShield());
            if (Utilities.Chance(3)) ChangeWing(EquipmentController.Ins.GetWing());
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();
        //agent.enabled = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        ChangeAnim(Anim.IDLE);
    }

    public override void Moving()
    {
        base.Moving();
        ChangeAnim(Anim.RUN);
        Patrol();
        //agent.enabled = true;
        agent.isStopped = false;
    }

    //public bool IsMoveFinish()
    //{
    //    return agent.remainingDistance <= 0.1f;
    //}

    public bool IsDestination => Vector3.Distance(TF.position, destination) - Mathf.Abs(TF.position.y - destination.y) < 0.1f;

    public void SetDestination(Vector3 point)
    {
        destination = point;
        agent.isStopped = false;
        agent.enabled = true;
        agent.SetDestination(destination);
        ChangeAnim(Anim.RUN);
    }

    public void Patrol()
    {   
        while (true)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                destination = point;
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

    public override void AddTarget(Character target)
    {
        base.AddTarget(target);

        if (!IsDead && Utilities.Chance(80, 100) && IsCanRunning)
        {
            ChangeState(new AttackState());
        }
    }

    //TODO:
    public void SetMark(bool r)
    {
        mask.SetActive(r);
    }

    public void HandleBeginDead()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    //public void HandleEndDead()
    //{
    //    if (LevelManager.Ins.RemainBot < 1)
    //    {
    //        HBPool.Despawn(this);
    //    }
    //    else
    //    {
    //        LevelManager.Ins.RemainBot -= 1;
    //        LevelManager.Ins.Alive -= 1;
    //        UIManager.Ins.GetUI<GamePlay>().SetAliveText(LevelManager.Ins.Alive);
    //        OnInit();
    //        ResetEQ1();
    //        Vector3 centre = LevelManager.Ins.GetRandomSpawnPos();
    //        while (true)
    //        {
    //            Vector3 point;
    //            if (RandomPoint(centre, 8f, out point))
    //            {
    //                //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
    //                ChangeState(new IdleState());
    //                TF.position = point;
    //                break;
    //            }

    //        }
    //    }
    //}

    public override void OnDespawn()
    {
        base.OnDespawn();
        //SpawnManager.Ins.BackColor(this.ColorSkin.material);
        HBPool.Despawn(this);
        CancelInvoke();
    }

    public override void DoDead(string name)
    {
        this.SetMark(false);
        ChangeState(null);
        StopMoving();
        base.DoDead();
        Invoke(nameof(OnDespawn), 2f);
    }

    //public override void AddTarget(Character target)
    //{
    //    base.AddTarget(target);
    //    ChangeState(new AttackState());
    //}

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

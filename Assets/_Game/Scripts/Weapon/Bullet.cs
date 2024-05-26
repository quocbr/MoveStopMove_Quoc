using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bullet : GameUnit
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected Transform child;
    //[SerializeField] protected Rigidbody rb;

    protected float currentMoveSpeed;

    protected CounterTime counterTime = new CounterTime();

    protected bool isRunning;

    protected Character attacker;
    protected Action<Character, Character> onHit;
    // set bullet data for bullet
    public virtual void OnInit(Material material, Character attacker, Action<Character, Character> onHit)
    {
        currentMoveSpeed = moveSpeed + ((attacker is Player)?moveSpeed * attacker.Buff.buffAttackSpeed/100f:0);
        this.attacker = attacker;
        this.onHit = onHit;
        ChangeMaterial(material);
        TF.localScale = attacker.TF.localScale;
        TF.forward = attacker.TF.forward;
        isRunning = true;
        if (attacker.EatedGiftBox == true)
        {
            currentMoveSpeed += currentMoveSpeed * 0.1f;
        }
    }

    protected virtual void Update()
    {
        //Move();
    }

    protected virtual void MoveForward()
    {
        TF.Translate(TF.forward * currentMoveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void MoveRotate()
    {
        TF.Translate(TF.forward * currentMoveSpeed * Time.deltaTime, Space.World);
        child.Rotate(Vector3.forward * -6, Space.Self);
    }

    public void OnDespawn()
    {
        HBPool.Despawn(this);
    }

    protected virtual void OnStop() { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);

            //if (attacker.Equals(victim) || victim.IsDead)
            //{
            //    return;
            //}
            if (victim != null && victim != attacker && !victim.IsDead)
            {
                ParticlePool.Play(ParticleType.BulletTrigger,this.TF.position,Quaternion.identity);
                onHit?.Invoke(attacker, victim);
                if(attacker is Player)
                {
                    SaveLoadManager.Ins.UserData.countKill += 1;
                }
                OnDespawn();
            }
            //onHit?.Invoke(attacker, victim);
            //OnDespawn();
        }

        //if (other.CompareTag(Constant.TAG_BLOCK))
        //{
        //    OnStop();
        //}
    }
    protected void ChangeMaterial(Material material)
    {
        if (material == null) return;

        Material[] materials = meshRenderer.materials;
        for (int i = 0; i < this.meshRenderer.materials.Length; i++)
        {
            materials[i] = material;
        }
        meshRenderer.materials = materials;
    }
}

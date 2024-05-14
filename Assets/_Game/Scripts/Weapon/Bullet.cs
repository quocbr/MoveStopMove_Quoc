using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bullet : GameUnit
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform child;
    //[SerializeField] protected Rigidbody rb;

    protected CounterTime counterTime = new CounterTime();

    protected bool isRunning;

    protected Character attacker;
    protected Action<Character, Character> onHit;
    // set bullet data for bullet
    public virtual void OnInit(Material material, Character attacker, Action<Character, Character> onHit)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        ChangeMaterial(material);
        TF.localScale = attacker.TF.localScale;
        TF.forward = attacker.TF.forward;
        isRunning = true;
    }

    protected virtual void Update()
    {
        //Move();
    }

    protected virtual void MoveForward()
    {
        TF.Translate(TF.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void MoveRotate()
    {
        TF.Translate(TF.forward * moveSpeed * Time.deltaTime, Space.World);
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
            if (victim != null && victim != attacker)
            {
                onHit?.Invoke(attacker, victim);
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

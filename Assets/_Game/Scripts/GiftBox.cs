using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : GameUnit
{
    [SerializeField] Animator animator;

    public void OnDespawn()
    {
        animator.ResetTrigger(Anim.GIFT_OPEN);
        SpawnManager.Ins.HanldeSpawnGiftBox();
        HBPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            //other
            animator.SetTrigger(Anim.GIFT_OPEN);
            Cache.GetCharacter(other).HandleEatGiftBox();
            Invoke(nameof(OnDespawn),2.2f);
        }
    }
}

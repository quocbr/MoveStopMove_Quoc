using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] private Character character;

    public void HandleBeginAttack()
    {
        character.Attacking();
    }

    public void HandleEndAttack()
    {
        character.ResetAttack();
    }

    public void HandleBotEndDead()
    {
        if(character is Bot)
        {
            (character as Bot).ChangeAnim(Anim.IDLE);
            (character as Bot).HandleEndDead();
            
        }
    }
}

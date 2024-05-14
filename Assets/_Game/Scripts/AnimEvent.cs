using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] private Character character;

    public void HandleBeginAttack()
    {
        //character.Attacking();
        character.CurrentWeapon.gameObject.SetActive(false);
    }

    public void HandleEndAttack()
    {
        //character.ResetAttack();
        character.CurrentWeapon.gameObject.SetActive(true);
    }

    public void HandleBotEndDead()
    {
        if(character is Bot)
        {
            ///(character as Bot).ChangeAnim(Anim.IDLE);
            
            
        }
    }
}

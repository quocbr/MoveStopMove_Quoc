using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSight : MonoBehaviour
{
    [SerializeField] Character character;
    private Vector3 originScale;

    private void Awake()
    {
        originScale = transform.localScale;
    }

    public void SetRange(int range)
    {
        this.transform.localScale = originScale + originScale * range / 100f; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character target = Cache.GetCharacter(other);
            if (!target.IsDead)
            {
                character.AddTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character target = Cache.GetCharacter(other);
            character.RemoveTarget(target);
        }
    }
}

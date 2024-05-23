using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBuff : MonoBehaviour
{
    [SerializeField] private CharacterSight characterSight;
    public int buffAttackRange = 0;
    public int buffAttackSpeed = 0;
    public int buffMoveSpeed = 0;
    public int buffGold = 0;

    public void HandleBuff(Character character)
    {
        buffAttackRange = 0;
        buffAttackSpeed = 0;
        buffMoveSpeed = 0;
        buffGold = 0;

        EquipmentData data;
        data = EquipmentController.Ins.GetWeapon(character.CurrentWeapon.poolType);
        HandleAddBuff(data.buff, data.value);
        if (character.CurrentHead != null)
        {
            data = EquipmentController.Ins.GetHead(character.CurrentHead.poolType);
            HandleAddBuff(data.buff, data.value);
        }
        if (character.CurrentPant != PoolType.None)
        {
            data = EquipmentController.Ins.GetPant1(character.CurrentPant);
            HandleAddBuff(data.buff, data.value);
        }
        if (character.CurrentShield != null)
        {
            data = EquipmentController.Ins.GetShield(character.CurrentShield.poolType);
            HandleAddBuff(data.buff, data.value);
        }
        if (character.CurrentSet != SetType.None)
        {
            data = EquipmentController.Ins.GetSet(character.CurrentSet);
            HandleAddBuff(data.buff, data.value);
        }
    }

    public void HandleAddBuff(EquipBuffType buffType, int value)
    {
        switch (buffType)
        {
            case EquipBuffType.Range:
                this.buffAttackRange += value;
                characterSight.SetRange(buffAttackRange);
                break;
            case EquipBuffType.AttackSpeed:
                this.buffAttackSpeed += value;
                break;
            case EquipBuffType.MoveSpeed:
                this.buffMoveSpeed += value;
                break;
            case EquipBuffType.Gold:
                this.buffGold += value;
                break;
        }
    }
}

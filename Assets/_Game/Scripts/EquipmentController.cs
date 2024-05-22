using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : Singleton<EquipmentController>
{
    [SerializeField] private EquipmentSO equipmentSO;

    List<EquipmentData> equipment;

    public List<EquipmentData> GetEquipment(EquipmentType type)
    {

        switch (type)
        {
            case EquipmentType.Weapon:
                equipment = equipmentSO.eWeapon;
                break;
            case EquipmentType.Head:
                equipment = equipmentSO.eHead;
                break;
            case EquipmentType.Tail:
                equipment = equipmentSO.eTail;
                break;
            case EquipmentType.Pant:
                equipment = equipmentSO.ePant;
                break;
            case EquipmentType.Shield:
                equipment = equipmentSO.eShield;
                break;
            case EquipmentType.Wing:
                equipment = equipmentSO.eWing;
                break;
            case EquipmentType.Set:
                equipment = equipmentSO.eSet;
                break;
        }
        return equipment;
    }

    public PoolType GetWeapon()
    {
        List<EquipmentData> listWeapon = GetEquipment(EquipmentType.Weapon);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listWeapon.Count; i++)
        {
                list.Add(listWeapon[i]);
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? list[index].poolType : PoolType.None;
    }

    public EquipmentData GetWeapon(PoolType poolType)
    {
        List<EquipmentData> listWeapon = GetEquipment(EquipmentType.Weapon);
        for (int i = 0; i < listWeapon.Count; i++)
        {
            if (listWeapon[i].poolType == poolType)
            {
                return listWeapon[i];
            }
        }
        return null;
    }

    public PoolType GetHead()
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Head);
        List<EquipmentData> list = new List<EquipmentData>();
        for(int i = 0; i< listHead.Count; i++)
        {
            if (listHead[i].setType == SetType.None)
            {
                list.Add(listHead[i]);
            }
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? list[index].poolType : PoolType.None;
    }

    public PoolType GetHead(SetType setType)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Head);
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == setType)
            {
                return listHead[i].poolType;
            }
        }
        return PoolType.None;
    }

    public EquipmentData GetHead(PoolType poolType)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Head);
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].poolType == poolType)
            {
                return listHead[i];
            }
        }
        return null;
    }

    public PoolType GetTail()
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Tail);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == SetType.None)
            {
                list.Add(listHead[i]);
            }
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? list[index].poolType: PoolType.None;
    }
    
    public PoolType GetTail(SetType setType)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Tail);
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == setType)
            {
                return listHead[i].poolType;
            }
        }
        return PoolType.None;
    }

    public PoolType GetWing()
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Wing);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == SetType.None)
            {
                list.Add(listHead[i]);
            }
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? list[index].poolType : PoolType.None;
    } 
    public PoolType GetWing(SetType setType)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Wing);
       
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == setType)
            {
                return listHead[i].poolType;
            }
        }
        return PoolType.None;
    }

    public PoolType GetShield()
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Shield);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == SetType.None)
            {
                list.Add(listHead[i]);
            }
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? list[index].poolType : PoolType.None;
    }
    
    public PoolType GetShield(SetType setType)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Shield);

        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == setType)
            {
                return listHead[i].poolType;
            }
        }
        return PoolType.None;
    }

    public EquipmentData GetShield(PoolType pool)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Shield);

        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].poolType == pool)
            {
                return listHead[i];
            }
        }
        return null;
    }

    public Material GetPant() 
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Pant);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == SetType.None)
            {
                list.Add(listHead[i]);
            }
        }

        int index = Random.Range(0, list.Count);
        return list.Count > 0 ? listHead[index].materials[0] : null;
    }
    
    public Material GetPant(SetType setType) 
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Pant);

        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == setType)
            {
                return listHead[i].materials[0];
            }
        }

        return null;
    }
    public Material GetPant(PoolType type) 
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Pant);

        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].poolType == type)
            {
                return listHead[i].materials[0];
            }
        }
        return null;
    }

    public EquipmentData GetPant1(PoolType type)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Pant);

        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].poolType == type)
            {
                return listHead[i];
            }
        }
        return null;
    }

    public SetType GetSet()
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Set);
        List<EquipmentData> list = new List<EquipmentData>();
        for (int i = 0; i < listHead.Count; i++)
        {
             list.Add(listHead[i]);
        }

        int index = Random.Range(0, list.Count);
        return list[index].setType;
    }

    public EquipmentData GetSet(SetType type)
    {
        List<EquipmentData> listHead = GetEquipment(EquipmentType.Set);
        for (int i = 0; i < listHead.Count; i++)
        {
            if (listHead[i].setType == type)
            {
                return listHead[i];
            }
        }
        return null;
    }
}

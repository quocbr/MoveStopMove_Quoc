using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EquipItemState { None=0,Lock=1,Unlock=2,Equiped=3}

public class EqItemUI : MonoBehaviour
{
    [SerializeField] private RawImage imageItem;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject iconLock;
    [SerializeField] private GameObject iconUnlock;
    [SerializeField] private GameObject iconEquiped;


    public Button SelectButton { get => selectButton; set => selectButton = value; }

    public void OnInit(EquipmentData equipmentData, Action<EquipmentData> itemButtonClick)
    {
        this.imageItem.texture = equipmentData.image;
        switch (equipmentData.equipmenyType)
        {
            case EquipmentType.Head:
                if (SaveLoadManager.Ins.UserData.listHeadOwn.Contains(equipmentData.poolType))
                {
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentHead)
                    {
                        ChangeStateEquip(EquipItemState.Equiped);
                    }
                    else
                    {
                        ChangeStateEquip(EquipItemState.Unlock);
                    }
                }
                else
                {
                    ChangeStateEquip(EquipItemState.Lock);
                }
                break;
            case EquipmentType.Pant:
                if (SaveLoadManager.Ins.UserData.listPantOwn.Contains(equipmentData.poolType))
                {
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentPant)
                    {
                        ChangeStateEquip(EquipItemState.Equiped);
                    }
                    else
                    {
                        ChangeStateEquip(EquipItemState.Unlock);
                    }
                }
                else
                {
                    ChangeStateEquip(EquipItemState.Lock);
                }
                break;
            case EquipmentType.Shield:
                if (SaveLoadManager.Ins.UserData.listShieldOwn.Contains(equipmentData.poolType))
                {
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentShield)
                    {
                        ChangeStateEquip(EquipItemState.Equiped);
                    }
                    else
                    {
                        ChangeStateEquip(EquipItemState.Unlock);
                    }
                }
                else
                {
                    ChangeStateEquip(EquipItemState.Lock);
                }
                break;
            case EquipmentType.Set:
                if (SaveLoadManager.Ins.UserData.listSetOwn.Contains(equipmentData.setType))
                {
                    if (equipmentData.setType == SaveLoadManager.Ins.UserData.currentSet)
                    {
                        ChangeStateEquip(EquipItemState.Equiped);
                    }
                    else
                    {
                        ChangeStateEquip(EquipItemState.Unlock);
                    }
                }
                else
                {
                    ChangeStateEquip(EquipItemState.Lock);
                }
                break;
        }
        SelectButton.onClick.AddListener(() =>
        {
            itemButtonClick.Invoke(equipmentData);
        });
    }

    public void ChangeStateEquip(EquipItemState state)
    {
        switch (state)
        {
            case EquipItemState.Lock:
                iconLock.SetActive(true);
                iconEquiped.SetActive(false);
                iconUnlock.SetActive(false);
                break;
            case EquipItemState.Unlock:
                iconLock.SetActive(false);
                iconEquiped.SetActive(false);
                iconUnlock.SetActive(true);
                break;
            case EquipItemState.Equiped:
                iconLock.SetActive(false);
                iconEquiped.SetActive(true);
                iconUnlock.SetActive(false);
                break;
        }
    }
}

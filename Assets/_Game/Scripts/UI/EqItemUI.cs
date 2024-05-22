using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EquipItemState {Lock=0,Unlock=1,Equiped=2,Selected=3}

public class EqItemUI : MonoBehaviour
{
    [SerializeField] private RawImage imageItem;
    [SerializeField] private Button selectButton;
    [SerializeField] GameObject[] stateObjects;
    //[SerializeField] private GameObject iconLock;
    //[SerializeField] private GameObject iconUnlock;
    //[SerializeField] private GameObject iconEquiped;

    public EquipItemState currentState;


    public Button SelectButton { get => selectButton; set => selectButton = value; }

    public void OnInit(EquipmentData equipmentData, Action<EquipmentData, EqItemUI> itemButtonClick)
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
            itemButtonClick.Invoke(equipmentData,this);
        });
    }

    public void ChangeStateEquip(EquipItemState state)
    {
        for (int i = 0; i < stateObjects.Length; i++)
        {
            stateObjects[i].SetActive(false);
        }

        stateObjects[(int)state].SetActive(true);

        this.currentState = state;
        //switch (state)
        //{
        //    case EquipItemState.Lock:
        //        currentState = EquipItemState.Lock;
        //        iconLock.SetActive(true);
        //        iconEquiped.SetActive(false);
        //        iconUnlock.SetActive(false);
        //        break;
        //    case EquipItemState.Unlock:
        //        currentState = EquipItemState.Unlock;
        //        iconLock.SetActive(false);
        //        iconEquiped.SetActive(false);
        //        iconUnlock.SetActive(true);
        //        break;
        //    case EquipItemState.Equiped:
        //        currentState = EquipItemState.Equiped;
        //        iconLock.SetActive(false);
        //        iconEquiped.SetActive(true);
        //        iconUnlock.SetActive(false);
        //        break;
        //}
    }
}


using System.Collections.Generic;

using UnityEngine;


public class ItemSelectionUi : MonoBehaviour
{
    [SerializeField] private SkinShop skinShop;
    [SerializeField] private EqItemUI buttonPrefab;
    [SerializeField] private Transform parentPosition;

    private List<EqItemUI> listItemUI = new List<EqItemUI>();

    private void SpawnItem(EquipmentData equipmentData)
    {
        EqItemUI itemUi = Instantiate(buttonPrefab, parentPosition);
        //HBPool.Spawn<EqItemUI>(buttonPrefab.poolType);
        //itemUi.transform.SetParent(parentPosition, false);
        itemUi.OnInit(equipmentData, OnItemUIClickHandle);
        listItemUI.Add(itemUi);
        //Kiem tra xem skin la trang thai gi
        itemUi.ChangeStateEquip(GetState(equipmentData));
        if (itemUi.currentState == EquipItemState.Equiped)
        {
            itemUi.SelectButton.onClick.Invoke();
            itemUi.ChangeStateEquip(EquipItemState.Selected);
            skinShop.itemEquied = itemUi;
        }
    }

    public EquipItemState GetState(EquipmentData equipmentData)
    {
        switch (skinShop.SelectionButton)
        {
            case ButtonSelection.Head:
                if (SaveLoadManager.Ins.UserData.listHeadOwn.Contains(equipmentData.poolType))
                {
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentHead)
                    {
                        return EquipItemState.Equiped;
                    }
                    else
                    {
                        return EquipItemState.Unlock;
                    }
                }
                break;
            case ButtonSelection.Pant:
                if (SaveLoadManager.Ins.UserData.listPantOwn.Contains(equipmentData.poolType))
                {
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentPant)
                    {
                        return EquipItemState.Equiped;
                    }
                    else
                    {
                        return EquipItemState.Unlock;
                    }
                }
                break;
            case ButtonSelection.Shield:
                if (SaveLoadManager.Ins.UserData.listShieldOwn.Contains(equipmentData.poolType))
                {

                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentShield)
                    {
                        return EquipItemState.Equiped;
                    }
                    else
                    {
                        return EquipItemState.Unlock;
                    }
                }
                break;
            case ButtonSelection.Set:
                if (SaveLoadManager.Ins.UserData.listSetOwn.Contains(equipmentData.setType))
                {
                    if (equipmentData.setType == SaveLoadManager.Ins.UserData.currentSet)
                    {
                        return EquipItemState.Equiped;
                    }
                    else
                    {
                        return EquipItemState.Unlock;
                    }
                }
                break;
        }
        return EquipItemState.Lock;
    }

    private void OnItemUIClickHandle(EquipmentData equipmentData, EqItemUI item)
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);

        if (skinShop.currentItem != null)
        {
            skinShop.currentItem.ChangeStateEquip(GetState(skinShop.CurrentEquipmentData));
        }

        skinShop.currentItem = item;

        switch (skinShop.SelectionButton)
        {
            case ButtonSelection.Head:
                LevelManager.Ins.Player.ChangeHead(equipmentData.poolType,true);
                if (SaveLoadManager.Ins.UserData.listHeadOwn.Contains(equipmentData.poolType))
                {
                    skinShop.BuyButton.gameObject.SetActive(false);
                    skinShop.EquipButton.gameObject.SetActive(true);
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentHead)
                    {
                        skinShop.SetEquipText(Constant.EQUIPED_STRING);
                    }
                    else
                    {
                        skinShop.SetEquipText(Constant.EQUIP_STRING);
                    }
                }
                else
                {
                    skinShop.BuyButton.gameObject.SetActive(true);
                    skinShop.EquipButton.gameObject.SetActive(false);
                }
                break;
            case ButtonSelection.Pant:
                LevelManager.Ins.Player.ChangePant(equipmentData.poolType,true);
                if (SaveLoadManager.Ins.UserData.listPantOwn.Contains(equipmentData.poolType))
                {
                    skinShop.BuyButton.gameObject.SetActive(false);
                    skinShop.EquipButton.gameObject.SetActive(true);
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentPant)
                    {
                        skinShop.SetEquipText(Constant.EQUIPED_STRING);
                    }
                    else
                    {
                        skinShop.SetEquipText(Constant.EQUIP_STRING);
                    }
                }
                else
                {
                    skinShop.BuyButton.gameObject.SetActive(true);
                    skinShop.EquipButton.gameObject.SetActive(false);
                }
                break;
            case ButtonSelection.Shield:
                LevelManager.Ins.Player.ChangeShield(equipmentData.poolType, true);
                if (SaveLoadManager.Ins.UserData.listShieldOwn.Contains(equipmentData.poolType))
                {
                    skinShop.BuyButton.gameObject.SetActive(false);
                    skinShop.EquipButton.gameObject.SetActive(true);
                    if (equipmentData.poolType == SaveLoadManager.Ins.UserData.currentShield)
                    {
                        skinShop.SetEquipText(Constant.EQUIPED_STRING);
                    }
                    else
                    {
                        skinShop.SetEquipText(Constant.EQUIP_STRING);
                    }
                }
                else
                {
                    skinShop.BuyButton.gameObject.SetActive(true);
                    skinShop.EquipButton.gameObject.SetActive(false);
                }
                break;
            case ButtonSelection.Set:
                LevelManager.Ins.Player.ChangeSet(equipmentData.setType,true);
                if (SaveLoadManager.Ins.UserData.listSetOwn.Contains(equipmentData.setType))
                {
                    skinShop.BuyButton.gameObject.SetActive(false);
                    skinShop.EquipButton.gameObject.SetActive(true);
                    if (equipmentData.setType == SaveLoadManager.Ins.UserData.currentSet)
                    {
                        skinShop.SetEquipText(Constant.EQUIPED_STRING);
                    }
                    else
                    {
                        skinShop.SetEquipText(Constant.EQUIP_STRING);
                    }
                }
                else
                {
                    skinShop.BuyButton.gameObject.SetActive(true);
                    skinShop.EquipButton.gameObject.SetActive(false);
                }
                break;
        }

        skinShop.SetCostText(equipmentData.cost);
        skinShop.SetEquipBuffText(equipmentData.buff, equipmentData.value);
        item.ChangeStateEquip(EquipItemState.Selected);

        skinShop.CurrentEquipmentData = equipmentData;
    }

    public void SpawnItemList(EquipmentType equipmentType)
    {
        ResSpawnAllItemData();
        List<EquipmentData> equipmentData = EquipmentController.Ins.GetEquipment(equipmentType);
        for (int i = 0; i < equipmentData.Count; i++)
        {
            if (equipmentType == EquipmentType.Set)
            {
                SpawnItem(equipmentData[i]);
                continue;
            }
            if (equipmentData[i].setType == SetType.None)
            {
                SpawnItem(equipmentData[i]);
            }
        }
        //if (listItemUI.Count > 0)
        //{
        //    listItemUI[0].SelectButton.onClick.Invoke();
        //}
        switch(equipmentType)
        {
            case EquipmentType.Set:
                if(SaveLoadManager.Ins.UserData.listSetOwn.Count == 0)
                {
                    listItemUI[0].SelectButton.onClick.Invoke();
                }
                break;
            case EquipmentType.Pant:
                if (SaveLoadManager.Ins.UserData.listPantOwn.Count == 0)
                {
                    listItemUI[0].SelectButton.onClick.Invoke();
                }
                break;
            case EquipmentType.Shield:
                if (SaveLoadManager.Ins.UserData.listShieldOwn.Count == 0)
                {
                    listItemUI[0].SelectButton.onClick.Invoke();
                }
                break;
            case EquipmentType.Head:
                if (SaveLoadManager.Ins.UserData.listHeadOwn.Count == 0)
                {
                    listItemUI[0].SelectButton.onClick.Invoke();
                }
                break;
        }
    }

    public void ResSpawnAllItemData()
    {
        for (int i = 0; i < listItemUI.Count; i++)
        {
            //HBPool.Despawn(listItemUI[i]);
            Destroy(listItemUI[i].gameObject);
        }
        listItemUI.Clear();
    }

}

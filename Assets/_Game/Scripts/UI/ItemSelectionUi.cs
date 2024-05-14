using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ItemSelectionUi : MonoBehaviour
{
    [SerializeField] private SkinShop skinShop;
    [SerializeField]private EqItemUI buttonPrefab;
    [SerializeField] private Transform parentPosition;

    private List<EqItemUI> listItemUI = new List<EqItemUI>();

    private void OnEnable()
    {
        if (listItemUI.Count > 0)
        {
            listItemUI[0].SelectButton.onClick.Invoke();
        }
    }

    private void SpawnItem(EquipmentData equipmentData)
    {
        EqItemUI itemUi = Instantiate(buttonPrefab, parentPosition);
        //HBPool.Spawn<EqItemUI>(buttonPrefab.poolType);
        //itemUi.transform.SetParent(parentPosition, false);
        itemUi.OnInit(equipmentData, OnItemUIClickHandle);
        listItemUI.Add(itemUi);
    }

    private void OnItemUIClickHandle(EquipmentData equipmentData)
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        switch (skinShop.SelectionButton)
        {
            case ButtonSelection.Head:
                LevelManager.Ins.Player.ChangeHead(equipmentData.poolType);
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
                LevelManager.Ins.Player.ChangePant(equipmentData.poolType);
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
                LevelManager.Ins.Player.ChangeShield(equipmentData.poolType);
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
                LevelManager.Ins.Player.ChangeSet(equipmentData.setType);
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
        skinShop.CurrentEquipmentData = equipmentData;
    }

    public void SpawnItemList(EquipmentType equipmentType)
    {
        ResSpawnAllItemData();
        List<EquipmentData> equipmentData = EquipmentController.Ins.GetEquipment(equipmentType);
        for(int i = 0; i< equipmentData.Count; i++)
        {
            if(equipmentType == EquipmentType.Set)
            {
                SpawnItem(equipmentData[i]);
                continue;
            }
            if(equipmentData[i].setType == SetType.None)
            {
                SpawnItem(equipmentData[i]);
            }
        }
        if(listItemUI.Count > 0)
        {
            listItemUI[0].SelectButton.onClick.Invoke();
        }
    }

    public void ResSpawnAllItemData()
    {
        for(int i = 0;i< listItemUI.Count; i++)
        {
            //HBPool.Despawn(listItemUI[i]);
            Destroy(listItemUI[i].gameObject);
        }
        listItemUI.Clear();
    }

}

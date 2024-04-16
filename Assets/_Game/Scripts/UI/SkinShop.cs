using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public enum ButtonSelection{ None =0 ,Head,Pant,Shield,Set}
public class SkinShop : UICanvas
{
    [SerializeField] private Button headButton;
    [SerializeField] private Button pantButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private Button setButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;

    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI equipText;
    [SerializeField] private ItemSelectionUi itemSelectionUi;

    private ButtonSelection selectionButton;
    private EquipmentData currentEquipmentData;

    public ButtonSelection SelectionButton { get => selectionButton;}
    public Button BuyButton { get => buyButton; }
    public Button EquipButton { get => equipButton;}
    public EquipmentData CurrentEquipmentData { get => currentEquipmentData; set => currentEquipmentData = value; }

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseButtonClickHandle);
        headButton.onClick.AddListener(OnHeadButtonClickhandle);
        pantButton.onClick.AddListener(OnPantButtonClickhandle);
        shieldButton.onClick.AddListener(OnShieldButtonClickhandle);
        setButton.onClick.AddListener(OnSetButtonClickhandle);

        buyButton.onClick.AddListener(OnBuyButtonClickHandle);
        equipButton.onClick.AddListener(OnEquipButtonClickHandle);
    }

    private void OnSetButtonClickhandle()
    {
        if(selectionButton == ButtonSelection.Set)
        {
            return;
        }
        headButton.image.color = Color.gray;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.black;
        selectionButton = ButtonSelection.Set;
        itemSelectionUi.SpawnItemList(EquipmentType.Set);
    }

    private void OnShieldButtonClickhandle()
    {
        if (selectionButton == ButtonSelection.Shield)
        {
            return;
        }
        headButton.image.color = Color.gray;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.black;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Shield;
        itemSelectionUi.SpawnItemList(EquipmentType.Shield);
    }

    private void OnPantButtonClickhandle()
    {
        if (selectionButton == ButtonSelection.Pant)
        {
            return;
        }
        headButton.image.color = Color.gray;
        pantButton.image.color = Color.black;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Pant;
        itemSelectionUi.SpawnItemList(EquipmentType.Pant);
    }

    private void OnHeadButtonClickhandle()
    {
        if (selectionButton == ButtonSelection.Head)
        {
            return;
        }
        headButton.image.color = Color.black;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Head;
        itemSelectionUi.SpawnItemList(EquipmentType.Head);
    }

    private void Start()
    {
        OnHeadButtonClickhandle();
    }

    public void OnBuyButtonClickHandle()
    {
        int currentCoin = SaveLoadManager.Ins.UserData.Coin;
        int price = currentEquipmentData.cost;
        if (currentCoin >= price)
        {
            currentCoin -= price;
            SetCoinText(currentCoin);
            SaveLoadManager.Ins.UserData.Coin = currentCoin;
            switch (selectionButton)
            {
                case ButtonSelection.Head:
                    SaveLoadManager.Ins.UserData.listHeadOwn.Add(currentEquipmentData.poolType);
                    break;
                case ButtonSelection.Pant:
                    SaveLoadManager.Ins.UserData.listPantOwn.Add(currentEquipmentData.poolType);
                    break;
                case ButtonSelection.Shield:
                    SaveLoadManager.Ins.UserData.listShieldOwn.Add(currentEquipmentData.poolType);
                    break;
                case ButtonSelection.Set:
                    SaveLoadManager.Ins.UserData.listSetOwn.Add(currentEquipmentData.setType);
                    break;
            }
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            SetEquipText(Constant.EQUIP_STRING);
            SaveLoadManager.Ins.Save();
        }
    }

    public void OnEquipButtonClickHandle()
    {
        if (selectionButton == ButtonSelection.Head)
        {
            SaveLoadManager.Ins.UserData.currentHead = currentEquipmentData.poolType;
        }
        if (selectionButton == ButtonSelection.Pant)
        {
            SaveLoadManager.Ins.UserData.currentPant=currentEquipmentData.poolType;
        }
        if (selectionButton == ButtonSelection.Shield)
        {
            SaveLoadManager.Ins.UserData.currentShield = currentEquipmentData.poolType;

        }
        if (selectionButton == ButtonSelection.Set)
        {
            SaveLoadManager.Ins.UserData.currentSet = currentEquipmentData.setType;
        }
        SetEquipText(Constant.EQUIPED_STRING);
        SaveLoadManager.Ins.Save();
    }

    private void OnCloseButtonClickHandle()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);
        selectionButton = ButtonSelection.None;
        Close(0);
    }

    public override void Open()
    {
        base.Open();
        CameraFollow.Ins.ZoomInSkinShop();
        LevelManager.Ins.Player.ChangeAnim(Anim.DANCE);
        SetCoinText(SaveLoadManager.Ins.UserData.Coin);
    }

    public override void Close(float delayTime)
    {
        
        LevelManager.Ins.Player.ChangeAnim(Anim.IDLE);
        base.Close(delayTime);
    }

    public void SetEquipText(string equip)
    {
        equipText.text = equip;
    }

    public void SetCostText(int cost)
    {
        costText.text = cost.ToString();
    }

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
}

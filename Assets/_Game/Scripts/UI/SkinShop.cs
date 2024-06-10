using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonSelection { None = 0, Head, Pant, Shield, Set }
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
    [SerializeField] private TextMeshProUGUI equipBuffText;
    [SerializeField] private ItemSelectionUi itemSelectionUi;
    private ButtonSelection selectionButton;
    private EquipmentData currentEquipmentData;
    public EqItemUI currentItem;
    public EqItemUI itemEquied;
    public ButtonSelection SelectionButton { get => selectionButton; }
    public Button BuyButton { get => buyButton; }
    public Button EquipButton { get => equipButton; }
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
        itemEquied = null;
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (selectionButton == ButtonSelection.Set)
        {
            return;
        }
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);

        headButton.image.color = Color.gray;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.black;
        selectionButton = ButtonSelection.Set;
        itemSelectionUi.SpawnItemList(EquipmentType.Set);
    }
    private void OnShieldButtonClickhandle()
    {
        itemEquied = null;
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (selectionButton == ButtonSelection.Shield)
        {
            return;
        }
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);

        headButton.image.color = Color.gray;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.black;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Shield;
        itemSelectionUi.SpawnItemList(EquipmentType.Shield);
    }
    private void OnPantButtonClickhandle()
    {
        itemEquied = null;
        currentItem = null;
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (selectionButton == ButtonSelection.Pant)
        {
            return;
        }
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);

        headButton.image.color = Color.gray;
        pantButton.image.color = Color.black;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Pant;
        itemSelectionUi.SpawnItemList(EquipmentType.Pant);
    }
    private void OnHeadButtonClickhandle()
    {
        itemEquied = null;
        currentItem = null;
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (selectionButton == ButtonSelection.Head)
        {
            return;
        }
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);

        headButton.image.color = Color.black;
        pantButton.image.color = Color.gray;
        shieldButton.image.color = Color.gray;
        setButton.image.color = Color.gray;
        selectionButton = ButtonSelection.Head;
        itemSelectionUi.SpawnItemList(EquipmentType.Head);
    }
    private void OnCloseButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.Player.ResetEQ(SaveLoadManager.Ins.UserData);
        Close(0);
    }
    public void OnBuyButtonClickHandle()
    {
        int currentCoin = SaveLoadManager.Ins.UserData.Coin;
        int price = currentEquipmentData.cost;
        if (currentCoin >= price)
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
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
            SaveLoadManager.Ins.SaveTofile();
        }
        else
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.DEBUTTON_CLICK);
        }
    }
    public void OnEquipButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (selectionButton == ButtonSelection.Head)
        {
            SaveLoadManager.Ins.UserData.currentHead = currentEquipmentData.poolType;
            SaveLoadManager.Ins.UserData.currentSet = SetType.None;
        }
        if (selectionButton == ButtonSelection.Pant)
        {
            SaveLoadManager.Ins.UserData.currentPant = currentEquipmentData.poolType;
            SaveLoadManager.Ins.UserData.currentSet = SetType.None;
        }
        if (selectionButton == ButtonSelection.Shield)
        {
            SaveLoadManager.Ins.UserData.currentShield = currentEquipmentData.poolType;
            SaveLoadManager.Ins.UserData.currentSet = SetType.None;

        }
        if (selectionButton == ButtonSelection.Set)
        {
            SaveLoadManager.Ins.UserData.currentSet = currentEquipmentData.setType;
            SaveLoadManager.Ins.UserData.currentHead = PoolType.None;
            SaveLoadManager.Ins.UserData.currentPant = PoolType.None;
            SaveLoadManager.Ins.UserData.currentShield = PoolType.None;

        }
         

        if (itemEquied != null)
        {
            itemEquied.ChangeStateEquip(EquipItemState.Unlock);
        }

        currentItem.ChangeStateEquip(EquipItemState.Equiped);
        itemEquied = currentItem;
        //currentItem.SelectButton.onClick.Invoke();

        SetEquipText(Constant.EQUIPED_STRING);

        SaveLoadManager.Ins.SaveTofile();
    }
    public override void Open()
    {
        base.Open();
        //CameraFollow.Ins.ZoomInSkinShop();
        CameraFollowe.Ins.ChangeState(CameraFollowe.State.Shop);
        LevelManager.Ins.Player.ChangeAnim(Anim.DANCE);
        SetCoinText(SaveLoadManager.Ins.UserData.Coin);

        OnHeadButtonClickhandle();
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
    public void SetEquipBuffText(EquipBuffType buffType, int value)
    {
        if (buffType == EquipBuffType.None) return;

        string nameBuff = "";
        switch (buffType)
        {
            case EquipBuffType.Range:
                nameBuff = "Range";
                break;
            case EquipBuffType.MoveSpeed:
                nameBuff = "Move Speed";
                break;
            case EquipBuffType.Gold:
                nameBuff = "Gold";
                break;
            case EquipBuffType.AttackSpeed:
                nameBuff = "Attack Speed";
                break;
        }
        //weaponAttribute.text = "+" + attribute.ToString() + nameBuff;
        equipBuffText.text = $"+{value}% {nameBuff}";
    }
}

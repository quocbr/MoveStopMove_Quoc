using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI coinText; 
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponAttribute;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [SerializeField] private Transform weaponShowPos;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] TextMeshProUGUI equipText;
    
    private EquipmentData dataWeapon;
    private GameUnit weaponShow;
    private List<EquipmentData> listWeapon = new List<EquipmentData>();
    private int page = 0;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyButtonClickHandle);
        equipButton.onClick.AddListener(OnEquipButtoClickHandle);

        nextButton.onClick.AddListener(NextButtonClickHandle);
        previousButton.onClick.AddListener(PrevButtonClickHandle);

        closeButton.onClick.AddListener(OnCloseButtonClickHandle);
    }

    private void OnCloseButtonClickHandle()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    protected override void OnInit()
    {
        base.OnInit();
        listWeapon = EquipmentController.Ins.GetEquipment(EquipmentType.Weapon);
        SetPageInformation(page);
    }

    public void SetPageInformation(int page)
    {
        if (weaponShow != null)
        {
            HBPool.Despawn(weaponShow);
        }
        dataWeapon = listWeapon[page];
        weaponName.text = dataWeapon.name.ToString();
        weaponShow = HBPool.Spawn<GameUnit>(dataWeapon.poolType);
        weaponShow.transform.SetParent(weaponShowPos,false);
        weaponShow.TF.localPosition = Vector3.zero;
        weaponShow.TF.localRotation = Quaternion.identity;
        weaponShow.TF.localScale = Vector3.one*2.5f;
        SetWeaponPrice(dataWeapon.cost);
        SetWeaponAttribute(dataWeapon.value);

        if (SaveLoadManager.Ins.UserData.ListWeaponOwn.Contains(dataWeapon.poolType)) 
        {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            if (SaveLoadManager.Ins.UserData.CurrentWeapon != dataWeapon.poolType)
            {
                SetEquipText(Constant.EQUIP_STRING);
            }
            else
            {
                SetEquipText(Constant.EQUIPED_STRING);
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }
    }
    public void SetWeaponPrice(int price)
    {
        weaponPrice.text = price.ToString();
    }
    public void SetWeaponAttribute(int attribute)
    {
        weaponAttribute.text = "+" + attribute.ToString() + "range";
    }

    public override void Open()
    {
        base.Open();
        LevelManager.Ins.Player.gameObject.SetActive(false);
        SetCoinText(SaveLoadManager.Ins.UserData.Coin);
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime);
        LevelManager.Ins.Player.gameObject.SetActive(true);
    }

    public void HomeButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0.5f);
    }

    public void OnEquipButtoClickHandle()
    {
        if (SaveLoadManager.Ins.UserData.ListWeaponOwn.Contains(dataWeapon.poolType))
        {
            SaveLoadManager.Ins.UserData.CurrentWeapon = dataWeapon.poolType;
            LevelManager.Ins.Player.ChangeWeapon(dataWeapon.poolType);
            SetEquipText(Constant.EQUIPED_STRING);
            SaveLoadManager.Ins.Save();
            UIManager.Ins.OpenUI<MainMenu>();
            Close(0);
        }
    }

    public void OnBuyButtonClickHandle()
    {
        if (dataWeapon.cost <= SaveLoadManager.Ins.UserData.Coin)
        {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            SaveLoadManager.Ins.UserData.Coin -= dataWeapon.cost;
            SetCoinText(SaveLoadManager.Ins.UserData.Coin);
            SaveLoadManager.Ins.UserData.ListWeaponOwn.Add(dataWeapon.poolType); 
            SetEquipText(Constant.EQUIP_STRING);
            SaveLoadManager.Ins.Save();
        }
    }
    private void NextButtonClickHandle()
    {
        if (page >= listWeapon.Count - 1)
        {
            return;
        }
        page++;
        SetPageInformation(page);
    }
    private void PrevButtonClickHandle()
    {
        if (page <= 0)
        {
            return;
        }
        page--;
        SetPageInformation(page);
    }
    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void SetEquipText(string text)
    {
        equipText.text = text;
    }
}

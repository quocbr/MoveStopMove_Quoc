using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject PanelSkinWeapon;

    [SerializeField] private TextMeshProUGUI coinText; 
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponAttribute;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [SerializeField] private Transform weaponShowPos;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] TextMeshProUGUI equipText;

    [SerializeField] private SkinWeaponUI buttonPrefab;
    [SerializeField] private Transform parentPosition;

    [SerializeField] private List<Transform> listWeaponSkinShowPos;
    [SerializeField] private List<RenderTexture> listRawImage;

    private List<SkinWeaponUI> listSkinWeaponUI = new List<SkinWeaponUI>();
    private List<Weapon> listSkinWeapon = new List<Weapon>();

    private EquipmentData dataWeapon;
    private GameUnit weaponShow;
    private List<EquipmentData> listWeapon = new List<EquipmentData>();
    private int page = 0;

    private Material cuurentMaterial;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyButtonClickHandle);
        equipButton.onClick.AddListener(OnEquipButtoClickHandle);

        nextButton.onClick.AddListener(NextButtonClickHandle);
        previousButton.onClick.AddListener(PrevButtonClickHandle);

        closeButton.onClick.AddListener(OnCloseButtonClickHandle);

        listWeapon = EquipmentController.Ins.GetEquipment(EquipmentType.Weapon);
    }

    private void OnCloseButtonClickHandle()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    protected override void OnInit()
    {
        base.OnInit();
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

        SpawnItemList(dataWeapon.poolType, dataWeapon.materials);

        if (SaveLoadManager.Ins.UserData.ListWeaponOwn.Contains(dataWeapon.poolType)) 
        {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            PanelSkinWeapon.SetActive(true);
            //TODO: Xu ly khi chon mau cho vu khi
            if (SaveLoadManager.Ins.UserData.CurrentWeapon != dataWeapon.poolType)
            {
                SetEquipText(Constant.EQUIP_STRING);
            }
            else if (dataWeapon.materials[0] == (LevelManager.Ins.Player.CurrentWeapon as Weapon).Material)
            {
                SetEquipText(Constant.EQUIPED_STRING);
            }
            else
            {
                SetEquipText(Constant.EQUIP_STRING);
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            PanelSkinWeapon.SetActive(false);
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
        SetPageInformation(page);
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime); 
        if (weaponShow != null)
        {
            HBPool.Despawn(weaponShow);
            weaponShow = null;
        }
        LevelManager.Ins.Player.gameObject.SetActive(true);
    }

    public void HomeButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0.5f);
    }

    public void OnEquipButtoClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        if (SaveLoadManager.Ins.UserData.ListWeaponOwn.Contains(dataWeapon.poolType))
        {
            SaveLoadManager.Ins.UserData.CurrentWeapon = dataWeapon.poolType;
            LevelManager.Ins.Player.ChangeWeapon(dataWeapon.poolType);
            (LevelManager.Ins.Player.CurrentWeapon as Weapon).ChangeMaterial(cuurentMaterial);
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
            SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            PanelSkinWeapon.SetActive(true);
            SaveLoadManager.Ins.UserData.Coin -= dataWeapon.cost;
            SetCoinText(SaveLoadManager.Ins.UserData.Coin);
            SaveLoadManager.Ins.UserData.ListWeaponOwn.Add(dataWeapon.poolType); 
            SetEquipText(Constant.EQUIP_STRING);
            SaveLoadManager.Ins.Save();
        }
        else
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.DEBUTTON_CLICK);
        }
    }
    private void NextButtonClickHandle()
    {
        if (page >= listWeapon.Count - 1)
        {
            return;
        }
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        page++;
        SetPageInformation(page);
    }
    private void PrevButtonClickHandle()
    {
        if (page <= 0)
        {
            return;
        }
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        page--;
        SetPageInformation(page);
    }

    private void SpawnItem(PoolType type,Material material,int index)
    {
        SkinWeaponUI itemUi = Instantiate(buttonPrefab, parentPosition);
        Weapon x = HBPool.Spawn<Weapon>(type);
        x.ChangeMaterial(material);

        
        x.transform.SetParent(listWeaponSkinShowPos[index],false);
        x.TF.localPosition = Vector3.zero;
        x.TF.localRotation = Quaternion.identity;
        x.TF.localScale = Vector3.one;
        
        itemUi.OnInit(material, listRawImage[index],OnItemUIClickHandle);
        listSkinWeapon.Add(x);
        listSkinWeaponUI.Add(itemUi);
    }

    private void OnItemUIClickHandle(Material material)
    {
        (weaponShow as Weapon).ChangeMaterial(material);
        cuurentMaterial = material;
        if(material == (LevelManager.Ins.Player.CurrentWeapon as Weapon).Material)
        {
            SetEquipText(Constant.EQUIPED_STRING);
        }
        else
        {
            SetEquipText (Constant.EQUIP_STRING);
        }
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
    }

    public void SpawnItemList(PoolType type,List<Material> listMaterialSkin)
    {
        ClearSkinListWeapon();
        
        for (int i = 0; i < listMaterialSkin.Count; i++)
        {
            SpawnItem(type,listMaterialSkin[i],i);
        }
        if(listSkinWeaponUI.Count > 0)
        {
            listSkinWeaponUI[0].SelectButton.onClick.Invoke();
        }
    }

    private void ClearSkinListWeapon()
    {
        for (int i = 0; i < listSkinWeaponUI.Count; i++)
        {
            //HBPool.Despawn(listItemUI[i]);
            listSkinWeapon[i].transform.parent = null;
            listSkinWeapon[i].transform.localScale = Vector3.one;
            HBPool.Despawn(listSkinWeapon[i]);
            Destroy(listSkinWeaponUI[i].gameObject);
            
        }
        listSkinWeaponUI.Clear();
        listSkinWeapon.Clear();
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

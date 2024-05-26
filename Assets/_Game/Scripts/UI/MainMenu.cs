using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : UICanvas
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponShopButton;
    [SerializeField] private Button skinShopButton;
    [SerializeField] private Button addFreeCoinButton;
    [SerializeField] private Button rankButton;
    [SerializeField] private Button logoutButton;

    [SerializeField] private TextMeshProUGUI cointText;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_InputField inputNameText;

    [SerializeField] private TextMeshProUGUI emailName;
    [SerializeField] private TextMeshProUGUI countKill;
    [SerializeField] private TextMeshProUGUI currentLevel;


    [SerializeField] private Toggle sFXToggle;

    private void Awake()
    {
        playButton.onClick.AddListener(OnMyPlayButtonClickHandle);
        weaponShopButton.onClick.AddListener(OnMyWeaponShopButtonClickHandle);
        skinShopButton.onClick.AddListener(OnMyShinShopButtonClickHandle);
        addFreeCoinButton.onClick.AddListener(OnMyAddFreeCoinButtonClickHandle);
        rankButton.onClick.AddListener(OnMyScoreboardButtonClickHandle);
        logoutButton.onClick.AddListener(OnMyLogoutButtonClickHandle);

        inputNameText.onEndEdit.AddListener(ChangeName);
        sFXToggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Ins.SoundFXOn();
            //SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        }
        else
        {
            SoundManager.Ins.SoundFXOff();
        }
        
    }

    private void OnMyShinShopButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<SkinShop>();
        Close(0);
    }
    
    private void OnMyAddFreeCoinButtonClickHandle()
    {
        AdsManager.Ins.onUserEarnedRewardCallback = () => {
            SaveLoadManager.Ins.UserData.Coin += 20;
            this.SetCoinText(SaveLoadManager.Ins.UserData.Coin);
            SaveLoadManager.Ins.SaveTofile();
        };
        AdsManager.Ins.ShowRewardedAd();
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollowe.Ins.ChangeState(CameraFollowe.State.MainMenu);
        //CameraFollow.Ins.ZoomIn();
        //Music and sound
        SoundManager.Ins.PlayMusic(Constant.MusicSound.HOME,true);
        if (SoundManager.Ins.isMute == true)
        {
            sFXToggle.isOn = false;
        }
        else
        {
            sFXToggle.isOn = true;
        }
        //Data
        this.SetCoinText(SaveLoadManager.Ins.UserData.Coin);
        this.SetEmail(SaveLoadManager.Ins.UserData.email);
        this.SetCurrentLevel(SaveLoadManager.Ins.UserData.currentLevel + 1);
        this.SetCountKill(SaveLoadManager.Ins.UserData.countKill);
        placeholder.text = SaveLoadManager.Ins.UserData.UserName;
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        SoundManager.Ins.StopMusic();
    }

    public void OnMyPlayButtonClickHandle()
    {
        LevelManager.Ins.OnStartGame();
        //CameraFollow.Ins.ZoomOut();
        CameraFollowe.Ins.ChangeState(CameraFollowe.State.Gameplay);
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }

    public void OnMyWeaponShopButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<WeaponShop>();
        Close(0);
    }

    private void OnMyScoreboardButtonClickHandle()
    {
        UIManager.Ins.OpenUI<Ranking>();
        Close(0);
    }

    private void OnMyLogoutButtonClickHandle()
    {
        AuthFirebase.Ins.LogOut();

        //LoadingMenuManager.Ins.SwitchToScene(0);
    }

    public void SetCoinText(int coin)
    {
        cointText.text = coin.ToString();
    }

    public void SetEmail(string email)
    {
        emailName.text = email;
    }

    public void SetCountKill(int kill)
    {
        countKill.text = kill.ToString();
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel.text = $"Current Level: {level}";
    }

    public void ChangeName(string name)
    {
        nameText.text = name;
        placeholder.text = nameText.text;
        SaveLoadManager.Ins.UserData.UserName = nameText.text;
        LevelManager.Ins.Player.SetNameChar(nameText.text);
        SaveLoadManager.Ins.SaveTofile();
    }
}

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

    [Header("Panel LogOut")]
    [SerializeField] private GameObject panelLogout;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;


    [SerializeField] private Toggle sFXToggle;
    [SerializeField] private Toggle vibrate;

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
        vibrate.onValueChanged.AddListener(ToggleVibrateValueChanged);
        
        yesButton.onClick.AddListener(() =>
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
            panelLogout.SetActive(false);
            AuthFirebase.Ins.LogOut();
        });
        noButton.onClick.AddListener(() =>
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
            panelLogout.SetActive(false);
        });


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
    private void ToggleVibrateValueChanged(bool isOn)
    {
        SoundManager.Ins.isOffVibrate = !isOn;
    }

    private void OnMyShinShopButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<SkinShop>();
        Close(0);
    }

    private void OnMyAddFreeCoinButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        AdsManager.Ins.onUserEarnedRewardCallback = () =>
        {
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
        SoundManager.Ins.PlayMusic(Constant.MusicSound.HOME, true);
        if (SoundManager.Ins.isMute == true)
        {
            sFXToggle.isOn = false;
        }
        else
        {
            sFXToggle.isOn = true;
        }
        vibrate.isOn = !SoundManager.Ins.isOffVibrate;
        //if (SoundManager.Ins.isOffVibrate == true)
        //{
        //    vibrate.isOn = true;
        //}
        //else
        //{
        //    vibrate.isOn = false;
        //}
        //Data
        this.SetCoinText(SaveLoadManager.Ins.UserData.Coin);
        this.SetEmail(SaveLoadManager.Ins.UserData.email);
        this.SetNamePlaceHold(SaveLoadManager.Ins.UserData.userName);
        this.SetCurrentLevel(SaveLoadManager.Ins.UserData.currentLevel + 1);
        this.SetCountKill(SaveLoadManager.Ins.UserData.countKill);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        SoundManager.Ins.StopMusic();
    }

    public void OnMyPlayButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
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
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<Ranking>();
        Close(0);
    }

    private void OnMyLogoutButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        //AuthFirebase.Ins.LogOut();
        panelLogout.SetActive(true);
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

    public void SetNamePlaceHold(string name)
    {
        placeholder.text = name;
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
        Debug.Log(name);
        nameText.text = name;
        placeholder.text = nameText.text;
        SaveLoadManager.Ins.UserData.UserName = nameText.text;
        LevelManager.Ins.Player.SetNameChar(nameText.text);
        SaveLoadManager.Ins.SaveTofile();
    }
}

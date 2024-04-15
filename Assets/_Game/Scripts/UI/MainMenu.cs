using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : UICanvas
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponShopButton;
    [SerializeField] private Button skinShopButton;

    [SerializeField] private TextMeshProUGUI cointText;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_InputField inputNameText;

    private void Awake()
    {
        playButton.onClick.AddListener(OnMyPlayButtonClickHandle);
        weaponShopButton.onClick.AddListener(OnMyWeaponShopButtonClickHandle);
        skinShopButton.onClick.AddListener(OnMyShinShopButtonClickHandle);

        inputNameText.onEndEdit.AddListener(delegate { ChangeName(); });
    }

    private void OnMyShinShopButtonClickHandle()
    {
        UIManager.Ins.OpenUI<SkinShop>();
        Close(0);
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollow.Ins.ZoomIn();

        //Data
        this.SetCoinText(SaveLoadManager.Ins.UserData.Coin);
        placeholder.text = SaveLoadManager.Ins.UserData.UserName;
    }

    public void OnMyPlayButtonClickHandle()
    {
        LevelManager.Ins.OnStartGame();
        CameraFollow.Ins.ZoomOut();
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }

    public void OnMyWeaponShopButtonClickHandle()
    {
        UIManager.Ins.OpenUI<WeaponShop>();
        Close(0);
    }

    public void SetCoinText(int coin)
    {
        cointText.text = coin.ToString();
    }

    public void ChangeName()
    {
        nameText.text = inputNameText.text;
        placeholder.text = nameText.text;
        SaveLoadManager.Ins.UserData.UserName = nameText.text;
        LevelManager.Ins.Player.NameChar = nameText.text;
        SaveLoadManager.Ins.Save();
    }
}

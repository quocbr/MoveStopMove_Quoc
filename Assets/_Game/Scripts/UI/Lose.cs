using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lose : UICanvas
{
    [SerializeField] private Button continueButton;

    [SerializeField] private TextMeshProUGUI textRank;
    [SerializeField] private TextMeshProUGUI nameKillerText;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    private void OnContinueButtonClick()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;
        LevelManager.Ins.OnRetry();
        Close(0);
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Lose);
        int alive = LevelManager.Ins.Alive;
        SetTextRank(alive);
        SetTextNameKiller(LevelManager.Ins.Player.KillByName);
       
        SetCoinText(LevelManager.Ins.Player.Point);
        SaveLoadManager.Ins.Save();

        UIManager.Ins.GetUI<GamePlay>().Close(0);
    }

    private void SetTextRank(int rank)
    {
        textRank.text = "#" + rank.ToString();
    }

    private void SetTextNameKiller(string nameKiller)
    {
        nameKillerText.text = nameKiller;
    }

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    } 
}

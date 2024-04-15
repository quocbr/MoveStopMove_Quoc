using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Win);

        SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;
        SetCoinText(LevelManager.Ins.Player.Point);
        SaveLoadManager.Ins.Save();

        UIManager.Ins.GetUI<GamePlay>().Close(0);
    }
    private void OnContinueButtonClick()
    {
        LevelManager.Ins.OnNextLevel();
        Close(0);
    }

    private void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
}

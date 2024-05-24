using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lose : UICanvas
{
    [SerializeField] private RectTransform levelFail, panelLose,coin;
    private Vector3 pos_levelFail, pos_panelLose,pos_Coin;
    private Vector3 sca_levelFail;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button rewardAdsButton;

    [SerializeField] private TextMeshProUGUI textRank;
    [SerializeField] private TextMeshProUGUI nameKillerText;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
        rewardAdsButton.onClick.AddListener(OnRewardAdsButtonClick);
    }

    protected override void OnInit()
    {
        base.OnInit();
        pos_levelFail = levelFail.localPosition;
        sca_levelFail = levelFail.localScale;

        pos_panelLose = panelLose.localPosition;

        pos_Coin = coin.localPosition;
    }

    private void OnContinueButtonClick()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        //SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;
        SaveLoadManager.Ins.SaveTofile();
        LevelManager.Ins.OnRetry();
        ResetAll();
        Close(0);
    }

    private void OnRewardAdsButtonClick()
    {
        AdsManager.Ins.onUserEarnedRewardCallback = () => {
            SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;
            SetCoinText(LevelManager.Ins.Player.Point * 2);

            rewardAdsButton.gameObject.SetActive(false);
            coin.DOAnchorPosX(0, 0.7f).SetDelay(0.5f).SetEase(Ease.OutCirc);
        };
        AdsManager.Ins.ShowRewardedAd();
    }

    public override void Open()
    {
        base.Open();

        //DoTween
        levelFail.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f).SetDelay(.2f).SetEase(Ease.OutElastic).OnComplete(LevelComplete);
        levelFail.DOAnchorPos(new Vector3(0, 700f, 0f), 0.7f).SetDelay(1.5f).SetEase(Ease.InOutCubic);
        levelFail.DOScale(new Vector3(1f, 1f, 1f), 2f).SetDelay(1.6f).SetEase(Ease.InOutCubic);


        GameManager.Ins.ChangeState(GameState.Lose);
        int alive = LevelManager.Ins.Alive;
        SetTextRank(alive);
        SetTextNameKiller(LevelManager.Ins.Player.KillByName);
        LevelManager.Ins.Player.Point += LevelManager.Ins.Player.Point * LevelManager.Ins.Player.Buff.buffGold / 100;
        SetCoinText(LevelManager.Ins.Player.Point);
        SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;

        //Music
        SoundManager.Ins.PlayMusic(Constant.MusicSound.LOSE);

        UIManager.Ins.GetUI<GamePlay>().Close(0);
    }

    private void SetTextRank(int rank)
    {
        textRank.text = $"#{rank}";
    }

    private void SetTextNameKiller(string nameKiller)
    {
        nameKillerText.text = nameKiller;
    }

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void LevelComplete()
    {
        panelLose.DOAnchorPos(new Vector3(0, 0f, 0f), 0.7f).SetDelay(0.5f).SetEase(Ease.OutCirc);
    }

    private void ResetAll()
    {
        levelFail.localPosition = pos_levelFail;
        levelFail.localScale = sca_levelFail;

        panelLose.localPosition = pos_panelLose;

        coin.localPosition = pos_Coin;

        rewardAdsButton.gameObject.SetActive(true);
    }
}

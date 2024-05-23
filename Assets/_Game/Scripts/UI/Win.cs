using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    [SerializeField] private RectTransform levelWin, panelWin, coloeWheel, coin;
    private Vector3 pos_levelWin, pos_panelWin,pos_Coin;
    private Vector3 sca_levelWin;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button rewardAdsButton;
    [SerializeField] private TextMeshProUGUI coinText;



    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
        rewardAdsButton.onClick.AddListener(OnRewardAdsButtonClick);
    }

    protected override void OnInit()
    {
        base.OnInit();
        pos_levelWin = levelWin.localPosition;
        sca_levelWin = levelWin.localScale;

        pos_levelWin = panelWin.localPosition;

        pos_Coin = coin.localPosition;

    }

    public override void Open()
    {
        base.Open();
        coloeWheel.DORotate(new Vector3(0, 0, -360f), 10f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart);
        levelWin.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f).SetDelay(.2f).SetEase(Ease.OutElastic).OnComplete(LevelComplete);
        levelWin.DOAnchorPos(new Vector3(0, 700f, 0f), 0.7f).SetDelay(1.5f).SetEase(Ease.InOutCubic);
        levelWin.DOScale(new Vector3(1f, 1f, 1f), 2f).SetDelay(1.6f).SetEase(Ease.InOutCubic);


        LevelManager.Ins.Player.ChangeAnim(Anim.WIN);
        GameManager.Ins.ChangeState(GameState.Win);
        LevelManager.Ins.Player.Point += LevelManager.Ins.Player.Point * LevelManager.Ins.Player.Buff.buffGold / 100;
        SaveLoadManager.Ins.UserData.Coin += LevelManager.Ins.Player.Point;
        SetCoinText(LevelManager.Ins.Player.Point);
        //SaveLoadManager.Ins.Save();
        //Music
        SoundManager.Ins.PlayMusic(Constant.MusicSound.WIN);
        UIManager.Ins.GetUI<GamePlay>().Close(0);
    }
    private void OnContinueButtonClick()
    {
        SaveLoadManager.Ins.Save();
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        LevelManager.Ins.OnNextLevel();
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

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void LevelComplete()
    {
        panelWin.DOAnchorPos(new Vector3(0, 0f, 0f), 0.7f).SetDelay(0.5f).SetEase(Ease.OutCirc);
    }

    private void ResetAll()
    {
        levelWin.localPosition = pos_levelWin;
        levelWin.localScale = sca_levelWin;

        panelWin.localPosition = pos_panelWin;

        coin.localPosition = pos_Coin;

        rewardAdsButton.gameObject.SetActive(true);
    }
}

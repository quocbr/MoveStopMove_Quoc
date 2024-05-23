using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Revive : UICanvas
{
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] TextMeshProUGUI counterTxt;
    private float counter;

    private void Awake()
    {
        reviveButton.onClick.AddListener(OnMyReviveButtonHandleClick);
        closeButton.onClick.AddListener(CloseButton);
    }

    public override void Open()
    {
        base.Open();
        StartCoroutine(nameof(CountDown));
    }

    public override void Setup()
    {
        base.Setup();
        GameManager.Ins.ChangeState(GameState.Revive);
        counter = 5;
    }

    //private void Update()
    //{
    //    //if (counter > 0)
    //    //{
    //    //    counter -= Time.deltaTime;
    //    //    counterTxt.SetText(counter.ToString("F0"));

    //    //    if (counter <= 0)
    //    //    {
    //    //        CloseButton();
    //    //        SoundManager.Ins.PlaySFX(Constant.SFXSound.COUNTDOWN_END);
    //    //    }
    //    //    else
    //    //    {
    //    //        SoundManager.Ins.PlaySFX(Constant.SFXSound.COUNTDOWN);
    //    //    }
    //    //}
    //}

    IEnumerator CountDown()
    {
        while (counter > 0)
        {
            counterTxt.SetText(counter.ToString("F0"));
            SoundManager.Ins.PlaySFX(Constant.SFXSound.COUNTDOWN);
            yield return new WaitForSeconds(1f);
            counter -= 1;
        }
        if (counter == 0)
        {
            SoundManager.Ins.PlaySFX(Constant.SFXSound.COUNTDOWN_END);
            CloseButton();
        }
        
    }

    public void OnMyReviveButtonHandleClick()
    {
        AdsManager.Ins.onUserEarnedRewardCallback = () => {
            GameManager.Ins.ChangeState(GameState.Gameplay);
            Close(0);
            LevelManager.Ins.OnRevive();
            UIManager.Ins.OpenUI<GamePlay>();
        };
        AdsManager.Ins.ShowRewardedAd();
    }

    public void CloseButton()
    {
        Close(0);
        LevelManager.Ins.Fail(0.2f);
    }
}

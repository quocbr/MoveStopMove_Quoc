using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : UICanvas
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button continueButton;

    [SerializeField] private Button soundOnButton;
    [SerializeField] private Button soundOffButton;
    [SerializeField] private Button vibOnButton;
    [SerializeField] private Button vibOffButton;

    private void Awake()
    {
        homeButton.onClick.AddListener(OnMyHomeButtonClickHandle);
        continueButton.onClick.AddListener(OnMyContinueButtonClickHandle);
        soundOnButton.onClick.AddListener(OnSoundOnButtonClickHandle);
        soundOffButton.onClick.AddListener(OnSoundOffButtonClickHandle);
        vibOnButton.onClick.AddListener (OnVibOnButtonClickHandle);
        vibOffButton.onClick.AddListener(OnVibOffButtonClickHandle);

        soundOffButton.onClick.Invoke();
    }

    public override void Open()
    {
        base.Open();
        if (SoundManager.Ins.isMute == true)
        {
            soundOnButton.gameObject.SetActive(true);
            soundOffButton.gameObject.SetActive(false);
        }
        else 
        {
            soundOnButton.gameObject.SetActive(false);
            soundOffButton.gameObject.SetActive(true);
        } 
    }

    private void OnMyHomeButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        LevelManager.Ins.OnRetry();
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    private void OnMyContinueButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }

    private void OnSoundOnButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        soundOnButton.gameObject.SetActive(false);
        soundOffButton.gameObject.SetActive(true);
        SoundManager.Ins.SoundFXOn();
    }

    private void OnSoundOffButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        soundOnButton.gameObject.SetActive(true);
        soundOffButton.gameObject.SetActive(false);
        SoundManager.Ins.SoundFXOff();
    }

    private void OnVibOnButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        vibOnButton.gameObject.SetActive(false);
        vibOffButton.gameObject.SetActive(true);
        //Handheld.Vibrate();
    }

    private void OnVibOffButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        vibOnButton.gameObject.SetActive(true);
        vibOffButton.gameObject.SetActive(false);
    }
}

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
    }

    private void OnMyHomeButtonClickHandle()
    {
        LevelManager.Ins.OnRetry();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    private void OnMyContinueButtonClickHandle()
    {
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }

    private void OnSoundOnButtonClickHandle()
    {
        soundOnButton.gameObject.SetActive(false);
        soundOffButton.gameObject.SetActive(true);
    }

    private void OnSoundOffButtonClickHandle()
    {
        soundOnButton.gameObject.SetActive(true);
        soundOffButton.gameObject.SetActive(false);
    }

    private void OnVibOnButtonClickHandle()
    {
        vibOnButton.gameObject.SetActive(false);
        vibOffButton.gameObject.SetActive(true);
    }

    private void OnVibOffButtonClickHandle()
    {
        vibOnButton.gameObject.SetActive(true);
        vibOffButton.gameObject.SetActive(false);
    }
}

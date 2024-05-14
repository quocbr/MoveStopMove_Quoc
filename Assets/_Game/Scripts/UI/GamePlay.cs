using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private TextMeshProUGUI aliveText;
    [SerializeField] private Button btnSetting;

    private void Awake()
    {
        btnSetting.onClick.AddListener(OnMySettingButtonClickHandle);
    }
    
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Gameplay);
        LevelManager.Ins.SetTargetIndicatorAlpha(1);
        GameManager.Ins.Joystick = joystick;
        SetAliveText(LevelManager.Ins.Alive);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        LevelManager.Ins.SetTargetIndicatorAlpha(0);
    }

    private void OnMySettingButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<Setting>();
        Close(0);
    }

    public void SetAliveText(int alive)
    {
        aliveText.text = "Alive: " + alive.ToString();
        aliveText.text = $"Alive: {alive}";
    }

}

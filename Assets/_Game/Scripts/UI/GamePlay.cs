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
        GameManager.Ins.Joystick = joystick;
        SetAliveText(LevelManager.Ins.Alive);
    }

    private void OnMySettingButtonClickHandle()
    {
        UIManager.Ins.OpenUI<Setting>();
        Close(0);
    }

    public void SetAliveText(int alive)
    {
        aliveText.text = "Alive: " + alive.ToString();
        aliveText.text = $"Alive: {alive}";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { None = 0,MainMenu, Gameplay, Setting, Win, Lose, Revive }

public class GameManager : Singleton<GameManager>
{
    private VariableJoystick joystick;
    private GameState gameState;

    public VariableJoystick Joystick { get => joystick; set => joystick = value; }

    private void Awake()
    {
        SaveLoadManager.Ins.OnInit();
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }

    public void Win()
    {
        UIManager.Ins.OpenUI<Win>();

    }
    public void Lose()
    {
        UIManager.Ins.OpenUI<Lose>();
    }

    [ContextMenu("Add Coin")]
    public void AddCoin()
    {
        SaveLoadManager.Ins.UserData.Coin += 100;
        SaveLoadManager.Ins.SaveTofile();
    }
}

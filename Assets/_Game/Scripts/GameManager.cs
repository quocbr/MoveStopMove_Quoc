using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Gameplay, Pause, Win, Lose}

public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

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


    [SerializeField] private VariableJoystick joystick;
    
    public VariableJoystick Joystick { get => joystick; set => joystick = value; }

    public void Win()
    {
        UIManager.Ins.OpenUI<Win>();

    }

    public void Lose()
    {
        UIManager.Ins.OpenUI<Lose>();
    }

}

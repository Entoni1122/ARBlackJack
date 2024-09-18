using System;
using UnityEngine;

public enum GameState { Players, Dealer }
public class GameManagers : MonoBehaviour
{
    GameState currentState;
    public GameState CurrentState { get { return currentState; } }
    public static GameManagers instance;

    public static Action OnDealerFreeWillCallBack;
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable()
    {
        UiManager.OnButtonRestartClickedCallBack += RestartGame;
    }
    private void OnDisable()
    {
        UiManager.OnButtonRestartClickedCallBack -= RestartGame;
    }
    public void RestartGame()
    {
        //When the restart is pressed do what u want
    }
    public void GameStateHandle()
    {
        switch (currentState)
        {
            case GameState.Players:
                break;
            case GameState.Dealer:
                OnDealerFreeWillCallBack?.Invoke();
                break;
            default:
                break;
        }
    }
    public void ChangeState(GameState state)
    {
        currentState = state;
        GameStateHandle();
    }
}

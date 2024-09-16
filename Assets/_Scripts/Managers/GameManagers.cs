using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : MonoBehaviour
{
    enum GameState { Players, Dealer }
    GameState currentState;

    [SerializeField] List<GameObject> playerInTheGame;
    int playerReadyCount;

    public static Action OnRestartGameCallBack;
    public static Action OnDealerFreeWillCallBack;
    
    int playerDoneCount;

    private void OnEnable()
    {
        BlackJackPlayer.OnPlayerReductionCallBack += PlayerReadyCount;
        BlackJackPlayer.OnPLayerTurnEndCallBack += PlayerFinished;
    }
    private void OnDisable()
    {
        BlackJackPlayer.OnPlayerReductionCallBack -= PlayerReadyCount;
        BlackJackPlayer.OnPLayerTurnEndCallBack -= PlayerFinished;
    }


    private void Start()
    {
        ChangeState(GameState.Dealer);
        playerDoneCount = playerInTheGame.Count;
    }
    //Checks when player are fully finished (bust, stand) so that the dealer can play second hand 
    public void PlayerFinished()
    {
        playerDoneCount--;
        if (playerDoneCount <= 0)
        {
            ChangeState(GameState.Dealer);
        }
    }
    
    public void PlayerReadyCount()
    {
        playerReadyCount++;
        print(playerReadyCount);

        if (playerReadyCount >= playerInTheGame.Count)
        {
            foreach (GameObject player in playerInTheGame)
            {
                player.GetComponentInChildren<BlackJackPlayer>().ChangeState(State.Decision);
            }
        }
    }

    public void GameStateHandle()
    {
        switch (currentState)
        {
            case GameState.Players:
                print("Players Turn");
                break;

            case GameState.Dealer:
                OnDealerFreeWillCallBack?.Invoke();
                print("DealersTurn");
                break;

            default:
                break;
        }
    }

    void ChangeState(GameState state)
    {
        currentState = state;
        GameStateHandle();
    }
}

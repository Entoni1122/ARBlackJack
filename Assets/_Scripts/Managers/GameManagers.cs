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
    int playerDone;

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
        playerDone = playerInTheGame.Count;
    }

    public void PlayerFinished()
    {
        playerDone--;
        if (playerDone <= 0)
        {
            OnDealerFreeWillCallBack?.Invoke();
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
                print("DealersTurn");
                break;

            default:
                break;
        }
    }

    void ChangeState(GameState state)
    {
        currentState = state;
    }
}

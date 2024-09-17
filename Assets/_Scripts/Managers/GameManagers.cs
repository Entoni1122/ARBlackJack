using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
        BlackJackDealer.OnCalculatePoints += CalculateWinner;
    }
    private void OnDisable()
    {
        BlackJackPlayer.OnPlayerReductionCallBack -= PlayerReadyCount;
        BlackJackPlayer.OnPLayerTurnEndCallBack -= PlayerFinished;
        BlackJackDealer.OnCalculatePoints -= CalculateWinner;
    }
    [SerializeField] List<GameObject> winners;
    [SerializeField] List<GameObject> playerTracked;
    int maxTableValue = 0;
    public void OnPlayerEnd()
    {
        for (int i = playerInTheGame.Count - 1; i >= 0; i--)
        {
            BlackJackPlayer playerComponent = playerInTheGame[i].GetComponentInChildren<BlackJackPlayer>();
            int playerPoints = playerComponent.Points;


            if (playerInTheGame[i].GetComponentInChildren<BlackJackPlayer>().CurrentState == State.Stand)
            {
                playerTracked.Add(playerInTheGame[i]);
                if (maxTableValue < playerPoints)
                {
                    maxTableValue = playerPoints;
                }
            }
        }
        winners = playerTracked;
    }
    private void Start()
    {
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

        if (playerReadyCount >= playerInTheGame.Count + 1)
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
                break;

            case GameState.Dealer:
                OnPlayerEnd();
                OnDealerFreeWillCallBack?.Invoke();
                break;

            default:
                break;
        }
    }
    public void CalculateWinner(int points)
    {
        //If the dealer got more then 21 calculate the winner form the player that are in stand
        //Otherwise calculate each time the dealer picks a card, who need to go out
        if (points > 21)
        {
            print("dealer lose");
            if (playerTracked.Count > 0)
            {
                for (int i = 0; i < playerTracked.Count; i++)
                {
                    print(playerTracked[i].GetComponentInChildren<BlackJackPlayer>().Names);
                }
            }
            return;
        }
        if (points == maxTableValue)
        {
            print("TIE");
        }
        if (points > maxTableValue)
        {
            if (playerTracked.Count > 0)
            {
                for (int i = 0; i < playerTracked.Count; i++)
                {
                    print(playerTracked[i].GetComponentInChildren<BlackJackPlayer>().Names + " Lost");
                }
            }
            return;
        }

        foreach (GameObject player in winners)
        {
            BlackJackPlayer playerComponent = player.GetComponentInChildren<BlackJackPlayer>();
            int playerPoints = playerComponent.Points;

            if (playerPoints < points || maxTableValue > playerPoints)
            {
                playerComponent.ChangeState(State.Bust);

                winners.Remove(player.gameObject);
                if (winners.Count <= 0)
                {
                    print("Dealer Win");
                }
            }
        }
    }

    void ChangeState(GameState state)
    {
        currentState = state;
        GameStateHandle();
    }
}

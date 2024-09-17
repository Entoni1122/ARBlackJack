using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagers : MonoBehaviour
{
    enum GameState { Players, Dealer }
    GameState currentState;

    [SerializeField] List<GameObject> playerInTheGame;
    int playerReadyCount;

    public static Action OnRestartGameCallBack;
    public static Action OnDealerFreeWillCallBack;

    [SerializeField] List<GameObject> winners;
    [SerializeField] List<GameObject> playerTracked;
    [SerializeField] GameObject winnerTextContainer;
    TextMeshProUGUI winnerText;
    int maxTableValue = 0;

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

    public void OnPlayerEnd()
    {
        //Checks who are the player still in game removing who is busted
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
        winnerText = winnerTextContainer.GetComponentInChildren<TextMeshProUGUI>();
        playerDoneCount = playerInTheGame.Count;
    }
    public void PlayerFinished()
    {
        //Checks when player are fully finished (bust, stand) so that the dealer can play second hand 
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

    public void ShowWinnerTEXT(string winner)
    {
        winnerText.text += " " + winner;
    }
    public void CalculateWinner(int points)
    {
        print("Caluclate winners");
        //If the dealer got more then 21 calculate the winner form the player that are in stand
        //Otherwise calculate each time the dealer picks a card, who need to go out
        if (points > 21)
        {
            if (playerTracked.Count > 0 && playerTracked.Count < 4)
            {
                for (int i = 0; i < playerTracked.Count; i++)
                {
                    //Put winner at the start of the sentence
                    winnerText.text += "Winners : ";
                    ShowWinnerTEXT(playerTracked[i].GetComponentInChildren<BlackJackPlayer>().Names);
                }
            }
            else
            {
                ShowWinnerTEXT("Dealer Busted");
            }
            winnerTextContainer.gameObject.SetActive(true);
            return;
        }
        if (points == maxTableValue)
        {
            winnerTextContainer.gameObject.SetActive(true);
            ShowWinnerTEXT("TIE");
        }
        if (points > maxTableValue)
        {
            if (playerTracked.Count > 0)
            {
                //for (int i = 0; i < playerTracked.Count; i++)
                //{
                //    print(playerTracked[i].GetComponentInChildren<BlackJackPlayer>().Names + " Lost");
                //}
                winnerTextContainer.gameObject.SetActive(true);
                ShowWinnerTEXT("Dealer Won");
            }
            return;
        }
        //Checks the remaining player to bust who is under the dealer
        for (int i = 0; i < winners.Count; i++)
        {
            BlackJackPlayer playerComponent = winners[i].GetComponentInChildren<BlackJackPlayer>();
            int playerPoints = playerComponent.Points;

            if (playerPoints < points || maxTableValue > playerPoints)
            {
                playerComponent.ChangeState(State.Bust);

                winners.Remove(winners[i].gameObject);

                //Check if the last player was removed so that the dealer wins without doing another time the method
                if (winners.Count <= 0)
                {
                    winnerTextContainer.gameObject.SetActive(true);
                    ShowWinnerTEXT("Dealer Won");
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

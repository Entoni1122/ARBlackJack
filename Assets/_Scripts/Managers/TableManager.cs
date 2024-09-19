using System;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("Reference in game")]
    [SerializeField] List<GameObject> playerInTheGame;
    [SerializeField] GameObject delearRef;
    [SerializeField] int playerThatHit;

    [Space]
    [Header("Debug")]
    [SerializeField] List<GameObject> winners;
    [SerializeField] List<GameObject> playerTracked;
    [SerializeField] int playerDoneCount;
    [SerializeField] int maxTableValue = 0;

    //Callbacks
    public static Action OnRestartGameCallBack;
    public static Action<string> OnShowWinnerCallBack;

    private void Start()
    {
        playerDoneCount = playerInTheGame.Count;
    }
    private void OnEnable()
    {
        BlackJackBaseActors.OnPlayerReductionCallBack += PlayerReadyCount;
        BlackJackPlayer.OnPlayerTurnEndCallBack += PlayerFinished;
        BlackJackDealer.OnCalculatePoints += CalculateWinner;
        UiManager.OnButtonRestartClickedCallBack += RestartGame;
    }
    private void OnDisable()
    {
        BlackJackBaseActors.OnPlayerReductionCallBack -= PlayerReadyCount;
        BlackJackPlayer.OnPlayerTurnEndCallBack -= PlayerFinished;
        BlackJackDealer.OnCalculatePoints -= CalculateWinner;
        UiManager.OnButtonRestartClickedCallBack -= RestartGame;
    }
    public void OnPlayerEnd()
    {
        print("All player ended");
        int dealerPoints = delearRef.GetComponent<BlackJackDealer>().Points;
        //Checks who are the player still in game removing who is busted
        for (int i = playerInTheGame.Count - 1; i >= 0; i--)
        {
            BlackJackPlayer playerComponent = playerInTheGame[i].GetComponentInChildren<BlackJackPlayer>();
            int playerPoints = playerComponent.Points;

            if (playerPoints <= 21 && playerPoints >= dealerPoints)
            {
                playerTracked.Add(playerInTheGame[i]);
                if (maxTableValue < playerPoints)
                {
                    maxTableValue = playerPoints;
                }
            }
        }
        winners = new List<GameObject>(playerTracked);
        CalculateWinner(dealerPoints);
    }
    public void PlayerFinished()
    {
        //Checks when player are fully finished (bust, stand) so that the dealer can play second hand 
        playerDoneCount--;
        if (playerDoneCount <= 0)
        {
            GameManagers.instance.ChangeState(GameState.Dealer);
            OnPlayerEnd();
        }
    }
    public void PlayerReadyCount()
    {
        playerThatHit++;

        if (playerThatHit >= playerDoneCount + 1)
        {
            foreach (GameObject player in playerInTheGame)
            {
                player.GetComponentInChildren<BlackJackPlayer>().ChangeState(State.Decision);
            }
            playerThatHit = 0;
        }
    }
    public void ShowWinnerTEXT(string winner)
    {
        OnShowWinnerCallBack?.Invoke(winner);
    }
    public void RestartGame()
    {
        GameManagers.instance.ChangeState(GameState.Players);
        playerThatHit = 0;
        maxTableValue = 0;
        playerDoneCount = playerInTheGame.Count;
        winners.Clear();
        playerTracked.Clear();
    }
    public void CalculateWinner(int dealerPoints)
    {
        //If the dealer got more then 21 calculate the winner form the player that are in stand
        //Otherwise calculate each time the dealer picks a card, who need to go out
        if (dealerPoints > 21)
        {
            //If there is more then one winner append one to another in the winner txt
            if (winners.Count > 1)
            {
                for (int i = 0; i < winners.Count; i++)
                {
                    //Put winner at the start of the sentence
                    ShowWinnerTEXT(winners[i].GetComponentInChildren<BlackJackPlayer>().Names);
                    winners[i].GetComponentInChildren<BlackJackPlayer>().ForceState(State.Win);
                }
            }
            else
            {
                ShowWinnerTEXT("Dealer Busted");
                winners[0].GetComponentInChildren<BlackJackPlayer>().ForceState(State.Win);
            }
            return;
        }
        if (dealerPoints > maxTableValue)
        {
            //If the dealer point are more than the table point but less then 21 it means that the dealer wind vs all
            //Then change state of the player to display something
            ShowWinnerTEXT("Dealer Won");
            for (int i = 0; i < winners.Count; i++)
            {
                winners[i].GetComponentInChildren<BlackJackPlayer>().ForceState(State.Bust);
            }
            return;
        }
        if (dealerPoints == maxTableValue)
        {
            ShowWinnerTEXT("TIE");
            for (int i = 0; i < winners.Count; i++)
            {
                winners[i].GetComponentInChildren<BlackJackPlayer>().ForceState(State.Tie);
            }
        }
        //Checks the remaining player to bust who is under the dealer
        for (int i = 0; i < winners.Count; i++)
        {
            BlackJackPlayer playerComponent = winners[i].GetComponentInChildren<BlackJackPlayer>();
            int playerPoints = playerComponent.Points;
            if (playerPoints < dealerPoints)
            {
                playerComponent.ForceState(State.Bust);
                winners.Remove(winners[i].gameObject);
                //Check if the last player was removed so that the dealer wins without doing another time the method
                if (winners.Count <= 0)
                {
                    playerComponent.ForceState(State.Bust);
                    ShowWinnerTEXT("Dealer Won");
                }
            }
        }
    }
}

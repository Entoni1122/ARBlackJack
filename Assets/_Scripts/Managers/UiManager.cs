using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("WinnerTXT")]
    [SerializeField] Button restartBTN;
    [SerializeField] GameObject winnerTextContainer;
    TextMeshProUGUI winnerText;

    [Header("PlayerStatTXT")]
    [SerializeField] GameObject playerStat;
    [SerializeField] TextMeshProUGUI playerNameTXT;
    [SerializeField] TextMeshProUGUI scoreTXT;
    [SerializeField] TextMeshProUGUI stateTXT;

    public static Action OnButtonRestartClickedCallBack;

    private void Awake()
    {
        winnerText = winnerTextContainer.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        restartBTN.onClick.AddListener(RestartButtonPressed);
    }
    private void OnEnable()
    {
        TableManager.OnShowWinnerCallBack += ShowWinnerTEXT;
        CardManager.OnZeroRemainingCards += ShowWinnerTEXT;
        BlackJackPlayer.OnPlayerClickedCallBack += ShowPlayerStats;
    }

    private void OnDisable()
    {
        TableManager.OnShowWinnerCallBack -= ShowWinnerTEXT;
        CardManager.OnZeroRemainingCards -= ShowWinnerTEXT;
        BlackJackPlayer.OnPlayerClickedCallBack -= ShowPlayerStats;
    }
    public void ShowWinnerTEXT(string winner)
    {
        winnerTextContainer.gameObject.SetActive(true);
        winnerText.text += "   " + winner;
        restartBTN.gameObject.SetActive(true);
    }
    void ShowPlayerStats(string name, int point,string state)
    {
        playerStat.SetActive(true);
        playerNameTXT.text = name;
        scoreTXT.text = point.ToString();
        stateTXT.text = state;
    }
    private void RestartButtonPressed()
    {
        print("Restar clicked");
        OnButtonRestartClickedCallBack?.Invoke();
        winnerText.text = "";
        restartBTN.gameObject.SetActive(false);
        winnerTextContainer.gameObject.SetActive(false);
    }
}

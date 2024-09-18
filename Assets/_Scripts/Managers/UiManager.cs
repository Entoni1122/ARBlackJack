using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Button restartBTN;
    [SerializeField] GameObject winnerTextContainer;
    TextMeshProUGUI winnerText;

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
    }

    private void OnDisable()
    {
        TableManager.OnShowWinnerCallBack -= ShowWinnerTEXT;
        CardManager.OnZeroRemainingCards -= ShowWinnerTEXT;
    }
    public void ShowWinnerTEXT(string winner)
    {
        winnerTextContainer.gameObject.SetActive(true);
        winnerText.text += "   " + winner;
        restartBTN.gameObject.SetActive(true);
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

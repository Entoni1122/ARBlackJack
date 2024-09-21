using System;
using System.Collections;
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

    [Header("StartGameObject")]
    [SerializeField] GameObject startGameContainer;
    [SerializeField] float duration;


    public static Action OnButtonRestartClickedCallBack;

    private void Awake()
    {
        winnerText = winnerTextContainer.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartGameTXT();
        restartBTN.onClick.AddListener(RestartButtonPressed);
    }
    private void StartGameTXT()
    {
        StartCoroutine(ScaleToY(new Vector3(1,0,1), duration));
    }
    private IEnumerator ScaleToY(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = startGameContainer.transform.localScale;
        float time = 0;

        while (time < duration)
        {
            startGameContainer.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        startGameContainer.transform.localScale = targetScale;
        if (startGameContainer.transform.localScale == targetScale)
        {
            startGameContainer.SetActive(false);
        }
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
    private void ShowPlayerStats(string name, int point,string state)
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

    bool isPaused;
    public void PauseGame()
    {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

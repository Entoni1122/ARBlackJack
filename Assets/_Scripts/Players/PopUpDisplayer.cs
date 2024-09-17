using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpDisplayer : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] GameObject popUp;
    [SerializeField] TextMeshProUGUI popUpTXT;
    [SerializeField] float duration;


    public void ShowTEXT(string text)
    {
        popUp.SetActive(true);
        popUpTXT.text = text;
        StartCoroutine(ShowTEXTTimer(duration));
    }

    IEnumerator ShowTEXTTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        popUp.SetActive(false);
    }
}

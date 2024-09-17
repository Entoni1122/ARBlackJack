using System;
using System.Collections;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] float pressedScale = 0.9f; 
    [SerializeField] float pressDuration = 0.1f; 
    [SerializeField] float returnDuration = 0.1f; 

    private Vector3 originalScale;    
    private bool isPressed = false;
    public static Action OnShuffleButtonPressed; 
    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (!isPressed) 
        {
            StartCoroutine(PressButton());
        }
    }

    private IEnumerator PressButton()
    {
        OnShuffleButtonPressed?.Invoke();
        //Kinda reproducing and animation similar to what Tween uses with the courutines to make an obj smaller and larger
        isPressed = true;
        yield return StartCoroutine(ScaleTo(originalScale * pressedScale, pressDuration));
        yield return new WaitForSeconds(pressDuration);
        yield return StartCoroutine(ScaleTo(originalScale, returnDuration));
        isPressed = false;
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float time = 0;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null; 
        }
        
        transform.localScale = targetScale;
    }
}

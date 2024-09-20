using System;
using System.Collections;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour, IOnClick
{
    [Header("Buttons Animation Variables")]
    [SerializeField] float pressedScale = 0.9f;
    [SerializeField] float pressDuration = 0.1f;
    [SerializeField] float returnDuration = 0.1f;
    
    [Header("AudioCLip")]
    [SerializeField] AudioClip clipPressedButton;
    [SerializeField] AudioClip clipShufflingCard;
    [SerializeField] Transform shufflePos;

    private Vector3 originalScale;
    private bool isPressed = false;

    public static Action OnShuffleButtonPressed;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private IEnumerator PressButton()
    {
        OnShuffleButtonPressed?.Invoke();
        //Kinda reproducing and animation similar to what Tween uses with the courutines to make an obj smaller and larger
        //probably going to use the same logic for UI animation
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

    public void OnClick()
    {
        if (!isPressed)
        {
            StartCoroutine(PressButton());
            AudioSource.PlayClipAtPoint(clipPressedButton, transform.position);
            AudioSource.PlayClipAtPoint(clipShufflingCard, shufflePos.position);
        }
    }

    public void OnClick(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public void OnClick(string name, int score)
    {
        throw new NotImplementedException();
    }
}

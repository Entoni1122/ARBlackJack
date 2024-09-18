using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlackJackBaseActors : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int points;
    [SerializeField] public string Names;
    protected int cardsInHand;
    public int Points { get { return points; } }

    [Header("CardBoucines")]
    [SerializeField] float bounceForce = 5f;
    [SerializeField] float upwardVelocity = 40f;
    [SerializeField] Vector3 bounceDirection;

    public static Action OnPlayerReductionCallBack;

    protected virtual void OnEnable()
    {
        UiManager.OnButtonRestartClickedCallBack += RestartGame;
    }
    protected virtual void OnDisable()
    {
        UiManager.OnButtonRestartClickedCallBack -= RestartGame;
    }
    public virtual void RestartGame()
    {
        print("BlackJackActor restarted");
        points = 0;
        cardsInHand = 0;
    }
    protected virtual void ThrowCard(Rigidbody rb)
    {
        float randomX = UnityEngine.Random.Range(5, 30);
        float randomZ = UnityEngine.Random.Range(-45, 45);
        bounceDirection = new Vector3(randomX, upwardVelocity, randomZ);
        Vector3 force = bounceDirection.normalized * bounceForce;

        rb.AddForce(force, ForceMode.Impulse);

        Debug.DrawRay(rb.transform.position, force, Color.red, 2.0f);

    }
}

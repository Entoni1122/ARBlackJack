using System;
using UnityEngine;
using UnityEngine.Diagnostics;

public enum State { Hit, Stand, Think, Bust, Decision }
public class BlackJackPlayer : BlackJackBaseActors
{
    [Header("Utils")]
    [SerializeField] Animator animator;
    [SerializeField] State currentState;
    [SerializeField] BoxCollider cardCatcher;
    [SerializeField] string stateTXT;
    int cardsInHand;

    
    public static Action OnPlayerReductionCallBack;
    public static Action OnPLayerTurnEndCallBack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ChangeState(State.Hit);
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Hit:
                //pop up they want to hit
                stateTXT = "hit";
                break;

            case State.Stand:
                stateTXT = "Stand";
                OnPlayerReductionCallBack?.Invoke();
                OnPLayerTurnEndCallBack?.Invoke();
                break;

            case State.Think:
                OnPlayerReductionCallBack?.Invoke();
                stateTXT = "think";

                break;

            case State.Decision:
                ChangeState(ShouldActBasedOnCardValue(points) ? State.Hit : State.Stand);
                print(points);
                stateTXT = "decision";

                break;
            case State.Bust:
                //pop up bust
                OnPlayerReductionCallBack?.Invoke();
                OnPLayerTurnEndCallBack?.Invoke();
                stateTXT = "bust";
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null || !rb.useGravity)
            {
                return;
            }
            if (currentState != State.Hit)
            {
                ThrowCard(rb);
                return;
            }

            points += other.GetComponent<CardValue>().Points;
            other.gameObject.layer = 0;
            cardsInHand++;

            if (points > 21)
            {
                ChangeState(State.Bust);
            }
            else if (cardsInHand == 2)
            {
                ChangeState(State.Think);
            }
        }
    }

    public void ChangeState(State states)
    {
        if (currentState != State.Stand && currentState != State.Bust)
        {
            currentState = states;
            HandleState();
        }
    }

    bool ShouldActBasedOnCardValue(float currentPoint)
    {
        float percentage = ((21.0f - currentPoint) / 21.0f) * 100.0f;

        int randomCheck = UnityEngine.Random.Range(0, 100);
        //if its under 10% i dont want the AI to hit regardless of their random decision
        if (percentage <= 10)
        {
            return false;
        }

        return randomCheck <= percentage;
    }
}

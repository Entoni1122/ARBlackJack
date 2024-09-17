using System;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

public enum State { Hit, Stand, Think, Bust, Decision }
public class BlackJackPlayer : BlackJackBaseActors
{
    [Header("Utils")]
    [SerializeField] Animator animator;
    [SerializeField] State currentState;
    public State CurrentState { get { return currentState; } }
    [SerializeField] BoxCollider cardCatcher;
    [SerializeField] string stateTXT;
    [SerializeField] PopUpDisplayer popUpDiplayer;
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
                stateTXT = "HIT";
                popUpDiplayer.ShowTEXT(stateTXT);
                break;

            case State.Stand:
                stateTXT = "STAND";
                popUpDiplayer.ShowTEXT(stateTXT);
                OnPlayerReductionCallBack?.Invoke();
                OnPLayerTurnEndCallBack?.Invoke();
                break;

            case State.Think:
                OnPlayerReductionCallBack?.Invoke();
                break;

            case State.Decision:
                if (ShouldHit(points))
                {
                    ChangeState(State.Hit);
                }
                else
                {
                    ChangeState(State.Stand);
                }
                break;
            case State.Bust:
                stateTXT = "BUST";
                popUpDiplayer.ShowTEXT(stateTXT);
                OnPlayerReductionCallBack?.Invoke();
                OnPLayerTurnEndCallBack?.Invoke();
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
            else if (points < 21 && cardsInHand > 2)
            {
                ChangeState(State.Decision);
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

    bool ShouldHit(float currentPoint)
    {
        //Just to have less probability that everyonw stand first turn (boring)
        if (points < 13)
        {
            return true;
        }
        else if (points > 19) //Just to not make the AI too dumb
        {
            return false;

        }
        float percentage = ((21.0f - currentPoint) / 21.0f) * 100.0f;
        int randomCheck = UnityEngine.Random.Range(0, 100);


        return randomCheck <= percentage;
    }
}

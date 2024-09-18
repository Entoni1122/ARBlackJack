using System;
using UnityEngine;

public enum State { Hit, Stand, Bust, Win, Think, Decision }
public class BlackJackPlayer : BlackJackBaseActors
{
    public State CurrentState { get { return currentState; } }

    [Header("Utils")]
    [SerializeField] State currentState;
    [SerializeField] Animator animator;
    [SerializeField] string stateTXT;
    [SerializeField] PopUpDisplayer popUpDiplayer;

    public static Action OnPlayerTurnEndCallBack;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentState = State.Hit;
    }
    public override void RestartGame()
    {
        base.RestartGame();
        currentState = State.Hit;
        ChangeState(State.Hit);
    }
    void HandleState()
    {
        //Each time you change state the animator sees the enum State and uses the correct number to switch
        animator.SetTrigger("ChangeState");
        animator.SetInteger("State",(int)currentState);
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
                OnPlayerTurnEndCallBack?.Invoke();
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
                OnPlayerTurnEndCallBack?.Invoke();
                break;
            default:
                break;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BlackJackPlayer : BlackJackBaseActors
{
    enum State { Hit, Stand, Think, Bust }

    Animator animator;
    State currentState;
    [SerializeField] BoxCollider cardCatcher;
    int cardsInHand;

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
                print("Hit");
                break;

            case State.Stand:
                print("Stand");
                break;

            case State.Think:
                ShouldActBasedOnCardValue(points);
                print("Thinking");
                break;

            case State.Bust:
                print("Bust");
                break;

            default:
                break;
        }
    }


    [SerializeField] float bounceForce = 10f;
    [SerializeField] Vector3 bounceDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null || !rb.useGravity)
            {
                return;
            }
            CardManager.instance.AddOutherCards(other.gameObject);
            CardManager.instance.RemoveInCards(other.gameObject);
            if (currentState != State.Hit)
            {
                float randomX = Random.Range(5, 30);
                float randomZ = Random.Range(-45, 45);
                bounceDirection = new Vector3(randomX, 10f, randomZ);
                Vector3 force = bounceDirection.normalized * bounceForce;

                rb.AddForce(force, ForceMode.Impulse);

                Debug.DrawRay(other.transform.position, force, Color.red, 2.0f);
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

    void ChangeState(State states)
    {
        currentState = states;
        HandleState();
    }

    bool ShouldActBasedOnCardValue(float currentPoint)
    {
        float percentage = ((21.0f - currentPoint) / 21.0f) * 100.0f;
        percentage = Mathf.Clamp(percentage, .0f, 100.0f);

        int randomCheck = Random.Range(0, 100);

        return randomCheck <= percentage;
    }
}

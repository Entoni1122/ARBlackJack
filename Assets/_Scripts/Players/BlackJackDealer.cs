using System;
using UnityEngine;

public class BlackJackDealer : BlackJackBaseActors
{
    bool isDealerTurn;
    bool isEndRound = false;

    public static Action<int> OnCalculatePoints;

    private void OnEnable()
    {
        GameManagers.OnDealerFreeWillCallBack += DealerCanPLay;
    }

    private void OnDisable()
    {
        GameManagers.OnDealerFreeWillCallBack -= DealerCanPLay;
    }

    public void DealerCanPLay()
    {
        isEndRound = true;
    }

    private void Start()
    {
        isDealerTurn = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (isEndRound)
            {
                points += other.GetComponent<CardValue>().Points;
                OnCalculatePoints?.Invoke(points);
                return;
            }
            if (!isDealerTurn)
            {
                ThrowCard(rb);
                return;
            }
            points += other.GetComponent<CardValue>().Points;
            cardsInHand++;
            if (cardsInHand == 2)
            {
                print(cardsInHand);
                OnPlayerReductionCallBack?.Invoke();
                isDealerTurn = false;
            }
        }
    }
}

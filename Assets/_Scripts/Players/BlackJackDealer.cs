using UnityEngine;

public class BlackJackDealer : BlackJackBaseActors
{
    bool isDealerTurn;
    bool isEndRound;

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
                if (points > 21)
                {
                    ThrowCard(rb);
                }
                //Winner callback
                return;
            }
            if (!isDealerTurn)
            {
                ThrowCard(rb);
                return;
            }
            points += other.GetComponent<CardValue>().Points;
            isDealerTurn = false;
        }
    }
}

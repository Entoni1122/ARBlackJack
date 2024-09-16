using UnityEngine;

public class BlackJackDealer : BlackJackBaseActors
{
    bool isDealerTurn;
    
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
        isDealerTurn = true;
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
            if (!isDealerTurn)
            {
                ThrowCard(rb);
                return;
            }
            points = other.GetComponent<CardValue>().Points;
            isDealerTurn = false;
        }
    }
}

using System;
using TMPro;
using UnityEngine;

public class BlackJackDealer : BlackJackBaseActors
{
    [SerializeField] TextMeshProUGUI dealerNameTXT;
    [SerializeField] TextMeshProUGUI dealerScoreTXT;

    bool isDealerTurn;
    bool isEndRound;

    public static Action<int> OnCalculatePoints;
    private void Awake()
    {
        RestartGame();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManagers.OnDealerFreeWillCallBack += DealerCanPLay;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManagers.OnDealerFreeWillCallBack -= DealerCanPLay;
    }

    public void DealerCanPLay()
    {
        isEndRound = true;
    }
    public override void RestartGame()
    {
        base.RestartGame();
        isDealerTurn = true;
        dealerNameTXT.text = Names;
        isEndRound = false;
        dealerScoreTXT.text = points.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            CardValue cardValue = other.GetComponent<CardValue>();
            if (isEndRound)
            {
                points += cardValue.Points;
                dealerScoreTXT.text = points.ToString();
                OnCalculatePoints?.Invoke(points);
                return;
            }
            if (!isDealerTurn)
            {
                ThrowCard(rb);
                return;
            }

            points += cardValue.Points;
            cardsInHand++;
            dealerScoreTXT.text = points.ToString();

            if (cardsInHand == 2)
            {
                OnPlayerReductionCallBack?.Invoke();
                isDealerTurn = false;
            }
        }
    }

}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [SerializeField] List<GameObject> inCards;
    [SerializeField] List<GameObject> outherCards;
    [SerializeField] float YOffSet;

    private void OnEnable()
    {
        TraceHandler.OnTakeFirstCardCallBack += SetLayerToCard;
        ButtonAnimation.OnShuffleButtonPressed += Shuffle;
    }
    private void OnDisable()
    {
        TraceHandler.OnTakeFirstCardCallBack -= SetLayerToCard;
        ButtonAnimation.OnShuffleButtonPressed -= Shuffle;

    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            inCards.Add(transform.GetChild(i).gameObject);
        }

        Shuffle();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shuffle();
        }
    }

#if UNITY_EDITOR
    //Method used with [ExecuteInEditMode] to assign the value to my card without doing it by hand
    public void GenerateValueInDeck()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int value = i % 13;
            if (value == 0)
            {
                value = 11;
            }
            else
            {
                value = Mathf.Clamp(value + 1, 2, 10);
            }
            transform.GetChild(i).AddComponent<CardValue>().Points = value;
        }
    }
#endif

    public void SetLayerToCard(GameObject obj)
    {
        if (inCards.Count <= 1)
        {
            return;

        }

        if (!inCards.Contains(obj))
        {
            return;
        }

        outherCards.Add(inCards[0]);


        inCards.RemoveAt(0);
        inCards[0].layer = LayerMask.NameToLayer("Interactable");
        inCards[0].GetComponent<BoxCollider>().enabled = true;
    }
    public void Shuffle()
    {
        inCards[0].layer = 0;
        inCards[0].GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < inCards.Count; i++)
        {
            int randomIndex = Random.Range(i, inCards.Count);

            GameObject tempCard = inCards[i];
            inCards[i] = inCards[randomIndex];
            inCards[randomIndex] = tempCard;

            Vector3 tempPosition = inCards[i].transform.position;
            inCards[i].transform.position = inCards[randomIndex].transform.position;
            inCards[randomIndex].transform.position = tempPosition;
        }
        inCards[0].layer = LayerMask.NameToLayer("Interactable");
        inCards[0].GetComponent<BoxCollider>().enabled = true;
    }
    public void GetAllCardTogheter()
    {
        print("Organise");
    }

}

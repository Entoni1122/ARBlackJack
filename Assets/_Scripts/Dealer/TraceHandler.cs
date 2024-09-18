using System;
using TMPro;
using UnityEngine;

public class TraceHandler : MonoBehaviour
{
    [Header("RayVARS")]
    [SerializeField] float traceLenght;
    [SerializeField] LayerMask layerToSlide;
    [SerializeField] float lerpSpeed;

    [Header("CardPosAndRot")]
    [SerializeField] float rotSpeed;
    [SerializeField] float yOffSet;

    [Header("PlayerStatTXT")]
    [SerializeField] GameObject playerStat;
    [SerializeField] TextMeshProUGUI playerNameTXT;
    [SerializeField] TextMeshProUGUI scoreTXT;
    GameObject objToMove;
    bool shouldTrace = true;

    public static Action<GameObject> OnTakeFirstCardCallBack;

    private void Update()
    {
        if (!shouldTrace)
        {
            return;
        }
        DealerRaycastCheck();
    }
    void DealerRaycastCheck()
    {
        RaycastHit hitData;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitData, traceLenght))
        {
            if (hitData.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                print("Overlappong with a player");
                playerStat.SetActive(true);
                playerNameTXT.text = hitData.collider.GetComponentInChildren<BlackJackPlayer>().Names;
                scoreTXT.text = hitData.collider.GetComponentInChildren<BlackJackPlayer>().Points.ToString();
                
            }
            else if (hitData.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")
                                                          && Input.GetMouseButtonDown(0))
            {
                objToMove = hitData.collider.gameObject;
                objToMove.GetComponent<Rigidbody>().isKinematic = false;
                objToMove.GetComponent<Rigidbody>().useGravity = false;

                OnTakeFirstCardCallBack?.Invoke(objToMove);
            }
            else
            {
                playerStat.SetActive(false);
            }
        }
        else
        {
            playerStat.SetActive(false);
        }

        if (!objToMove)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 targetPos = ray.GetPoint(hitData.distance) + new Vector3(0, yOffSet, 0);
            objToMove.transform.position = Vector3.Lerp(objToMove.transform.position,
                                                        targetPos,
                                                        lerpSpeed * Time.deltaTime);

            if (Input.GetMouseButton(1))
            {
                objToMove.transform.localEulerAngles = Vector3.Lerp(objToMove.transform.localEulerAngles,
                                                               objToMove.transform.localEulerAngles + Vector3.up,
                                                               rotSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            objToMove.GetComponent<Rigidbody>().useGravity = true;
            objToMove = null;
        }
    }
}

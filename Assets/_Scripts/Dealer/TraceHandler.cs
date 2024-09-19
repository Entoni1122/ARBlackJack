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
            IOnClick clickable = hitData.collider.gameObject.GetComponentInChildren<IOnClick>();
            if (clickable != null && Input.GetMouseButtonDown(0))
            {
                print("Overlapping with something clickable");
                clickable.OnClick();
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
                //Stats disale
            }
        }
        else
        {
            //Stats disale
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

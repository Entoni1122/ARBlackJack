using System;
using UnityEngine;

public class TraceHandler : MonoBehaviour
{
    [SerializeField] float traceLenght;
    [SerializeField] LayerMask layerToSlide;
    [SerializeField] float lerpSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float yOffSet;

    GameObject objToMove;

    public static Action<GameObject> OnTakeFirstCardCallBack;


    private void Update()
    {
        RaycastHit hitData;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hitData, traceLenght))
        {
            if (hitData.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")
                && Input.GetMouseButtonDown(0))
            {
                objToMove = hitData.collider.gameObject;
                objToMove.GetComponent<Rigidbody>().useGravity = false;

                OnTakeFirstCardCallBack?.Invoke(objToMove);
            }
        }

        if (!objToMove)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 targetPos = hitData.point;
            objToMove.transform.position = Vector3.Lerp(objToMove.transform.position,
                                                        targetPos + new Vector3(0,yOffSet,0),
                                                        lerpSpeed * Time.deltaTime);
            if (Input.GetMouseButton(1))
            {
                objToMove.transform.localEulerAngles = Vector3.Lerp(objToMove.transform.localEulerAngles, 
                                                               objToMove.transform.localEulerAngles + Vector3.up,
                                                               rotSpeed * Time.deltaTime);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            objToMove.GetComponent<Rigidbody>().useGravity = true;

            objToMove = null;
        }
    }
}

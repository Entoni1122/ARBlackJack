using UnityEditor.PackageManager.UI;
using UnityEditor.Rendering;
using UnityEngine;

public class HeadSimulation : MonoBehaviour
{
    [SerializeField] float lookSpeed;
    [SerializeField] float threshold = 50f;

    Vector3 startingRotation;

    private void Start()
    {
        startingRotation = transform.localEulerAngles;
    }
    private void Update()
    {
        MouseCheck();
    }

    public void MouseCheck()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 target = Vector3.zero;
        if (mousePos.x < threshold)
        {
            target.y = -20;
        }
        else if (mousePos.x > Screen.width - threshold)
        {
            target.y = 20;
        }
        else
        {
            target.y = 0;
        }
        if (mousePos.y < threshold)
        {
            target.x = 20;
        }
        else if (mousePos.y > Screen.height - threshold)
        {
            target.x = -20;
        }
        else
        {
            target.x = 0; 
        }
        target = startingRotation + target;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(target), lookSpeed * Time.deltaTime);
    }
}

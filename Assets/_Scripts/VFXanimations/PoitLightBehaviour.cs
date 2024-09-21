using UnityEngine;

public class PoitLightBehaviour : MonoBehaviour , IOnClick
{
    GameObject pointLight;
    [SerializeField] AudioClip clip;
    [SerializeField] Material lightOffMaterial;
    Material startingMaterial;
    bool isOn;

    private void Awake()
    {
        startingMaterial = GetComponent<MeshRenderer>().material;
        pointLight = transform.GetChild(0).gameObject;
        isOn = true;
    }

    public void OnClick()
    {
        if (isOn)
        {
            isOn = false;
            pointLight.SetActive(false);
            AudioSource.PlayClipAtPoint(clip,transform.position);
            gameObject.GetComponent<MeshRenderer>().material = lightOffMaterial;
        }
        else
        {
            isOn = true;
            pointLight.SetActive(true);
            AudioSource.PlayClipAtPoint(clip, transform.position);
            gameObject.GetComponent<MeshRenderer>().material = startingMaterial;
        }
    }

    public void OnClick(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public void OnClick(string name, int score)
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : MonoBehaviour, IOnClick
{
    [SerializeField] Light fireLight;
    [SerializeField] float minIntensity = 0.2f;
    [SerializeField] float maxIntensity = 1f;
    [SerializeField] float flickerSpeed = 1f;
    [SerializeField] float scaleMultiplier = 0.3f;

    private float fireTimer = 0f;

    bool isLightOn = true;


    public void OnClick()
    {
        if (isLightOn)
        {
            fireLight.gameObject.SetActive(false);
            isLightOn = false;
        }
        else
        {
            fireLight.gameObject.SetActive(true);
            isLightOn = true;
        }
    }

    public void OnClick(GameObject obj)
    {

    }

    public void OnClick(string name, int score)
    {

    }

    private void Start()
    {
        if (fireLight == null)
        {
            fireLight = GetComponentInChildren<Light>();
        }
    }
    private void Update()
    {
        FirePingPong();
    }

    //Simulates the flickering of the fire
    void FirePingPong()
    {
        fireTimer += Time.deltaTime * flickerSpeed;

        float fireFlicker = Mathf.Sin(fireTimer);

        if (fireLight != null)
        {
            fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (fireFlicker + 1) / 2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : MonoBehaviour
{
    [SerializeField] Light fireLight;
    [SerializeField] float minIntensity = 0.2f; 
    [SerializeField] float maxIntensity = 1f; 
    [SerializeField] float flickerSpeed = 1f;
    [SerializeField] float scaleMultiplier = 0.3f;

    private float fireTimer = 0f;

    private void Start()
    {
        if (fireLight == null)
        {
            fireLight = GetComponentInChildren<Light>();
        }
    }
    //Simulates the flickering of the fire
    private void Update()
    {
        fireTimer += Time.deltaTime * flickerSpeed;

        float fireFlicker = Mathf.Sin(fireTimer);

        if (fireLight != null)
        {
            fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (fireFlicker + 1) / 2);
        } 
    }
}

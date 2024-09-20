using UnityEngine;

public class FireAnimation : MonoBehaviour, IOnClick
{
    [Header("Fire Animation Variables")]
    [SerializeField] Light fireLight;
    [SerializeField] float minIntensity = 0.2f;
    [SerializeField] float maxIntensity = 1f;
    [SerializeField] float flickerSpeed = 1f;
    [SerializeField] float scaleMultiplier = 0.3f;

    [Header("AudioClip")]
    [SerializeField] AudioClip clipLighter;
    [SerializeField] AudioClip clipBlow;

    [Header("VFX")]
    [SerializeField] ParticleSystem fireParticle;

    private float fireTimer = 0f;

    bool isLightOn = true;

    private void Start()
    {
        if (fireLight == null)
        {
            fireLight = GetComponentInChildren<Light>();
        }
        fireParticle.Play();
        fireParticle.GetComponentInChildren<ParticleSystem>().Play();
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
    public void OnClick()
    {
        if (isLightOn)
        {
            fireLight.gameObject.SetActive(false);
            isLightOn = false;
            AudioSource.PlayClipAtPoint(clipBlow, transform.position);
            fireParticle.Stop();
            fireParticle.GetComponentInChildren<ParticleSystem>().Stop();
        }
        else
        {
            fireLight.gameObject.SetActive(true);
            isLightOn = true;
            AudioSource.PlayClipAtPoint(clipLighter, transform.position);
            fireParticle.Play();
            fireParticle.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void OnClick(GameObject obj)
    {

    }

    public void OnClick(string name, int score)
    {

    }
}

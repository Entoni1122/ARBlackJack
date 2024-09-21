using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource backGroundAS;



    private void Start()
    {
        backGroundAS.Play();
    }
}

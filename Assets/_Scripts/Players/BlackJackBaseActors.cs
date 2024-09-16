using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlackJackBaseActors : MonoBehaviour
{
    [SerializeField] protected int points;

    [SerializeField] float bounceForce = 10f;
    [SerializeField] Vector3 bounceDirection;

    protected virtual void ThrowCard(Rigidbody rb)
    {
        float randomX = UnityEngine.Random.Range(5, 30);
        float randomZ = UnityEngine.Random.Range(-45, 45);
        bounceDirection = new Vector3(randomX, 10f, randomZ);
        Vector3 force = bounceDirection.normalized * bounceForce;

        rb.AddForce(force, ForceMode.Impulse);

        Debug.DrawRay(rb.transform.position, force, Color.red, 2.0f);

    }
}

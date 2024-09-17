using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlackJackBaseActors : MonoBehaviour
{
    [SerializeField] protected int points;
    public int Points { get { return points; }  }
    [SerializeField] float bounceForce = 5f;
    [SerializeField] float upwardVelocity = 40f;
    [SerializeField] Vector3 bounceDirection;
    public static Action OnPlayerReductionCallBack;
    protected int cardsInHand;
    [SerializeField] public string Names;

    protected virtual void ThrowCard(Rigidbody rb)
    {
        float randomX = UnityEngine.Random.Range(5, 30);
        float randomZ = UnityEngine.Random.Range(-45, 45);
        bounceDirection = new Vector3(randomX, upwardVelocity, randomZ);
        Vector3 force = bounceDirection.normalized * bounceForce;

        rb.AddForce(force, ForceMode.Impulse);

        Debug.DrawRay(rb.transform.position, force, Color.red, 2.0f);

    }
}

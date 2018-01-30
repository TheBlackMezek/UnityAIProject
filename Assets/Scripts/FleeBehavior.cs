using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : MonoBehaviour {

    public float speed;

    public Transform target;
    public Rigidbody rb;

    private Vector3 desiredVelocity;



    private void Update()
    {
        desiredVelocity = speed * (transform.position - target.position).normalized;

        rb.AddForce(desiredVelocity - rb.velocity);
    }

}

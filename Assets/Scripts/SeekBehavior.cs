using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehavior : MonoBehaviour {

    public float speed;
    public float arriveThreshold = 2.0f;
    public float arriveSpeed = 1.0f;

    public Transform target;
    public Rigidbody rb;

    private Vector3 desiredVelocity;



    private void Update()
    {
        Vector3 projectedPos = transform.position + rb.velocity;

        float currentSpeed = speed;
        if (Vector3.Distance(target.position, transform.position) <= arriveThreshold)
        {
            currentSpeed = Mathf.Lerp(arriveSpeed, speed,
                           Vector3.Distance(target.position, transform.position) / arriveThreshold);
        }
        
        desiredVelocity = currentSpeed * (target.position - projectedPos).normalized;

        rb.AddForce(desiredVelocity - rb.velocity);
    }

}

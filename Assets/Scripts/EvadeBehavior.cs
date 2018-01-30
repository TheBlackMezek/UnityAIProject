using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeBehavior : MonoBehaviour {

    public float speed = 5.0f;
    public float predictionDist = 2.0f;
    public float fleeThreshold = 2.0f;

    public Transform target;
    public Rigidbody targetBody;
    public Rigidbody rb;

    private Vector3 desiredVelocity;



    private void Update()
    {
        //if (Vector3.Distance(target.position, transform.position) <= fleeThreshold)
        //{
        //    desiredVelocity = speed * (transform.position - target.position).normalized;
        //
        //    rb.AddForce(desiredVelocity - rb.velocity);
        //}
        //else
        //{
        //    Vector3 predictedPosition = target.position + targetBody.velocity * predictionDist;
        //    desiredVelocity = speed * (transform.position - predictedPosition).normalized;
        //
        //    rb.AddForce(desiredVelocity - rb.velocity);
        //}
        rb.AddForce(GetForce());
    }

    public Vector3 GetForce()
    {
        if (Vector3.Distance(target.position, transform.position) <= fleeThreshold)
        {
            desiredVelocity = speed * (transform.position - target.position).normalized;

            return desiredVelocity - rb.velocity;
        }
        else
        {
            Vector3 predictedPosition = target.position + targetBody.velocity * predictionDist;
            desiredVelocity = speed * (transform.position - predictedPosition).normalized;

            return desiredVelocity - rb.velocity;
        }
    }

}

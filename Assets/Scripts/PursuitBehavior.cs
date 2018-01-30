using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitBehavior : MonoBehaviour {

    public float speed = 5.0f;
    public float predictionDist = 2.0f;
    public float seekThreshold = 2.0f;
    public float arriveThreshold = 2.0f;
    public float arriveSpeed = 1.0f;

    public Transform target;
    public Rigidbody targetBody;
    public Rigidbody rb;

    private Vector3 desiredVelocity;



    private void Update()
    {
        //float currentSpeed = speed;
        //if(Vector3.Distance(target.position, transform.position) <= arriveThreshold)
        //{
        //    currentSpeed = Mathf.Lerp(speed, arriveSpeed,
        //                   Vector3.Distance(target.position, transform.position) / arriveThreshold);
        //}
        //
        //if(Vector3.Distance(target.position, transform.position) <= seekThreshold)
        //{
        //    Vector3 projectedPos = transform.position + rb.velocity;
        //
        //    desiredVelocity = currentSpeed * (target.position - projectedPos).normalized;
        //
        //    rb.AddForce(desiredVelocity - rb.velocity);
        //}
        //else
        //{
        //    Vector3 predictedPosition = target.position + targetBody.velocity * predictionDist;
        //    desiredVelocity = currentSpeed * (predictedPosition - transform.position).normalized;
        //
        //    rb.AddForce(desiredVelocity - rb.velocity);
        //}
        rb.AddForce(GetForce());
    }

    public Vector3 GetForce()
    {
        float currentSpeed = speed;
        if (Vector3.Distance(target.position, transform.position) <= arriveThreshold)
        {
            currentSpeed = Mathf.Lerp(speed, arriveSpeed,
                           Vector3.Distance(target.position, transform.position) / arriveThreshold);
        }

        if (Vector3.Distance(target.position, transform.position) <= seekThreshold)
        {
            Vector3 projectedPos = transform.position + rb.velocity;

            desiredVelocity = currentSpeed * (target.position - projectedPos).normalized;

            return desiredVelocity - rb.velocity;
        }
        else
        {
            Vector3 predictedPosition = target.position + targetBody.velocity * predictionDist;
            desiredVelocity = currentSpeed * (predictedPosition - transform.position).normalized;

            return desiredVelocity - rb.velocity;
        }
    }

}

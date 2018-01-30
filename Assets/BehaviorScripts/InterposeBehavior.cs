using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterposeBehavior : MonoBehaviour
{

    public float speed = 5.0f;
    public float predictionMultiplier = 1.0f;
    public float arriveThreshold = 2.0f;
    public float arriveSpeed = 1.0f;

    public Transform target1;
    public Rigidbody targetBody1;
    public Transform target2;
    public Rigidbody targetBody2;
    public Rigidbody rb;

    private Vector3 desiredVelocity;



    public Vector3 GetForce()
    {
        Vector3 predictedPos1 = (target1.position + targetBody1.velocity) * predictionMultiplier;
        Vector3 predictedPos2 = (target2.position + targetBody2.velocity) * predictionMultiplier;

        Vector3 target = new Vector3(Mathf.Lerp(predictedPos1.x, predictedPos2.x, 0.5f),
                                     Mathf.Lerp(predictedPos1.y, predictedPos2.y, 0.5f),
                                     Mathf.Lerp(predictedPos1.z, predictedPos2.z, 0.5f));

        Vector3 projectedPos = transform.position + rb.velocity;

        float currentSpeed = speed;
        if (Vector3.Distance(target, transform.position) <= arriveThreshold)
        {
            currentSpeed = Mathf.Lerp(arriveSpeed, speed,
                           Vector3.Distance(target, transform.position) / arriveThreshold);
        }

        desiredVelocity = currentSpeed * (target - projectedPos).normalized;

        return desiredVelocity - rb.velocity;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum RangedState
{
    SHOOT,
    APPROACH,
    FLEE
}

public class FSMRanged : MonoBehaviour {

    public float range = 5.0f;
    public float speed = 5.0f;

    public Rigidbody body;
    public Transform target;
    public MeshRenderer mr;

    public Material shootMat;
    public Material appMat;
    public Material fleeMat;

    private RangedState state;
    private float arriveThreshold;
    private float arriveSpeed = 0.0f;
    private float fleeRange;

    private Vector3 desiredVelocity;




    // Use this for initialization
    void Start () {
        state = RangedState.APPROACH;
        mr.material = appMat;
        arriveThreshold = range + 1f;
        fleeRange = range - 2f;
	}
	
	// Update is called once per frame
	void Update () {
		switch(state)
        {
            case RangedState.APPROACH:
                Vector3 projectedPos = transform.position + body.velocity;

                float currentSpeed = speed;
                if (Vector3.Distance(target.position, transform.position) <= arriveThreshold)
                {
                    currentSpeed = Mathf.Lerp(arriveSpeed, speed,
                                   Vector3.Distance(target.position, transform.position) / arriveThreshold);
                }

                desiredVelocity = currentSpeed * (target.position - projectedPos).normalized;

                body.AddForce(desiredVelocity - body.velocity);

                if(Vector3.Distance(transform.position, target.position) <= range)
                {
                    state = RangedState.SHOOT;
                    mr.material = shootMat;
                }
                break;

            case RangedState.SHOOT:

                Vector3 midTrg = target.position -
                                 (target.position - transform.position).normalized * range;
                projectedPos = transform.position + body.velocity;
                Debug.DrawLine(transform.position, midTrg, Color.black);

                currentSpeed = speed;
                if (Vector3.Distance(midTrg, transform.position) <= arriveThreshold)
                {
                    currentSpeed = Mathf.Lerp(arriveSpeed, speed,
                                   Vector3.Distance(midTrg, transform.position) / arriveThreshold);
                }

                desiredVelocity = currentSpeed * (midTrg - projectedPos).normalized;

                body.AddForce(desiredVelocity - body.velocity);

                if (Vector3.Distance(transform.position, target.position) > arriveThreshold)
                {
                    state = RangedState.APPROACH;
                    mr.material = appMat;
                }
                else if (Vector3.Distance(transform.position, target.position) <= fleeRange)
                {
                    state = RangedState.FLEE;
                    mr.material = fleeMat;
                }

                break;

            case RangedState.FLEE:
                desiredVelocity = speed * (transform.position - target.position).normalized;

                body.AddForce(desiredVelocity - body.velocity);

                if (Vector3.Distance(transform.position, target.position) > fleeRange)
                {
                    state = RangedState.SHOOT;
                    mr.material = shootMat;
                }

                break;
        }
	}
}

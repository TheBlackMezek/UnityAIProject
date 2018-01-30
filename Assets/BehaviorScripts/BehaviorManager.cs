using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour {

    public Rigidbody body;

    [Range(0.0f, 1.0f)]
    public float pursuitMultiplier = 0.0f;
    [Range(0.0f, 1.0f)]
    public float wanderMultiplier = 1.0f;
    [Range(0.0f, 1.0f)]
    public float evadeMultiplier = 0.0f;
    [Range(0.0f, 1.0f)]
    public float interposeMultiplier = 0.0f;
    [Range(0.0f, 1.0f)]
    public float flockingMultiplier = 0.0f;

    public PursuitBehavior pursuit;
    public WanderBehavior wander;
    public EvadeBehavior evade;
    public InterposeBehavior interpose;
    public FlockingBehavior flocking;



    // Update is called once per frame
    void Update () {
        Vector3 force = Vector3.zero;
        if (pursuit)
        {
            force += pursuit.GetForce() * pursuitMultiplier;
        }
        if(wander)
        {
            force += wander.GetForce() * wanderMultiplier;
        }
        if (evade)
        {
            force += evade.GetForce() * evadeMultiplier;
        }
        if (interpose)
        {
            force += interpose.GetForce() * interposeMultiplier;
        }
        if (flocking)
        {
            force += flocking.GetForce() * flockingMultiplier;
        }

        body.AddForce(force);
	}

}

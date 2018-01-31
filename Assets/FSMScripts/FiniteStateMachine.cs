using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum States
{
    WANDER,
    FLEE,
    REST
}


public class FiniteStateMachine : MonoBehaviour {

    public float transitionTime = 1.0f;
    public float maxEnergy = 5.0f;

    public WanderBehavior wander;
    public FleeBehavior flee;
    public MeshRenderer mr;
    public Material wanderMaterial;
    public Material fleeMaterial;

    private States state = States.WANDER;
    private float timer;
    private float energy;




	// Use this for initialization
	void Start () {
        timer = transitionTime;
        energy = maxEnergy;

        state = States.WANDER;
        mr.material = wanderMaterial;
        wander.enabled = true;
        flee.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        //timer -= Time.deltaTime;
        //
        //if(timer <= 0)
        //{
        //    timer = transitionTime;
        //    switch(state)
        //    {
        //        case States.WANDER:
        //            state = States.FLEE;
        //            mr.material = fleeMaterial;
        //            flee.enabled = true;
        //            wander.enabled = false;
        //            break;
        //        case States.FLEE:
        //            state = States.WANDER;
        //            mr.material = wanderMaterial;
        //            wander.enabled = true;
        //            flee.enabled = false;
        //            break;
        //    }
        //}
        
        switch(state)
        {
            case States.WANDER:
                energy -= Time.deltaTime;
                if(energy <= 0)
                {
                    wander.enabled = false;
                    state = States.REST;
                    mr.material = fleeMaterial;
                }
                break;
            case States.REST:
                energy += Time.deltaTime;
                if(energy >= maxEnergy)
                {
                    energy = maxEnergy;
                    wander.enabled = true;
                    state = States.WANDER;
                    mr.material = wanderMaterial;
                }
                break;
        }

	}
}

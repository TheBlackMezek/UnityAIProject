using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {

    public float speed = 5.0f;

    public Rigidbody body;

    private Vector3 desiredVelocity;
	

	
	// Update is called once per frame
	void Update () {
        Vector3 forceVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        desiredVelocity = speed * forceVec.normalized;

        body.AddForce(desiredVelocity - body.velocity);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : MonoBehaviour {

    public float speed = 5.0f;
    public float arrivalTheshold = 0.5f;
    public float minWanderLength = 2.0f;
    public float maxWanderLength = 5.0f;
    public float tooSlowThreshold = 0.1f;
    
    public Rigidbody rb;

    private Vector3 target;
    private Vector3 desiredVelocity;



    private void Start()
    {
        float length = Random.Range(minWanderLength, maxWanderLength);
        target = transform.position + rb.velocity.normalized * length;

        Vector2 circlepos = Random.insideUnitCircle;
        target = new Vector3(target.x + circlepos.x, transform.position.y, target.z + circlepos.y);
    }

    private void Update()
    {
        //if(target == null || Vector3.Distance(target, transform.position) <= arrivalTheshold)
        //{
        //    float length = Random.Range(minWanderLength, maxWanderLength);
        //    target = transform.position + rb.velocity.normalized * length;
        //
        //    Vector2 circlepos = Random.insideUnitCircle;
        //    target = new Vector3(target.x + circlepos.x, transform.position.y, target.z + circlepos.y);
        //}
        //
        //if(rb.velocity.magnitude <= tooSlowThreshold)
        //{
        //    float length = Random.Range(minWanderLength, maxWanderLength);
        //    target = transform.position;
        //
        //    Vector2 circlepos = Random.insideUnitCircle * length;
        //    target = new Vector3(target.x + circlepos.x, transform.position.y, target.z + circlepos.y);
        //}
        //
        ////Debug.DrawLine(transform.position, target, Color.green);
        //desiredVelocity = speed * (target - transform.position).normalized;

        rb.AddForce(GetForce());
    }

    public Vector3 GetForce()
    {
        if (target == null || Vector3.Distance(target, transform.position) <= arrivalTheshold)
        {
            float length = Random.Range(minWanderLength, maxWanderLength);
            target = transform.position + rb.velocity.normalized * length;

            Vector2 circlepos = Random.insideUnitCircle;
            target = new Vector3(target.x + circlepos.x, transform.position.y, target.z + circlepos.y);
        }

        if (rb.velocity.magnitude <= tooSlowThreshold)
        {
            float length = Random.Range(minWanderLength, maxWanderLength);
            target = transform.position;

            Vector2 circlepos = Random.insideUnitCircle * length;
            target = new Vector3(target.x + circlepos.x, transform.position.y, target.z + circlepos.y);
        }
        
        desiredVelocity = speed * (target - transform.position).normalized;

        return desiredVelocity - rb.velocity;
    }

}

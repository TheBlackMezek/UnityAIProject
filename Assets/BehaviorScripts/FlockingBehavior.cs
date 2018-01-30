using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingBehavior : MonoBehaviour {

    public GameObject flock;
    public float speed = 5.0f;
    public float alignmentRange = 5.0f;
    public float cohesionRange = 5.0f;
    public float separationRange = 3.0f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.0f;

    public Rigidbody body;




    public Vector3 GetForce()
    {
        Vector3 alignment = ComputeAlignment();
        Vector3 cohesion = ComputeCohesion();
        Vector3 separation = ComputeSeparation();

        Vector3 force = alignment * alignmentWeight
                      + cohesion * cohesionWeight
                      + separation * separationWeight;
        force = force.normalized * speed;

        return force;
    }



    public Vector3 ComputeAlignment()
    {
        Vector3 point = Vector3.zero;
        int neighborCount = 0;


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alignmentRange);
        foreach(Collider c in hitColliders)
        {
            if(c.gameObject != gameObject && c.gameObject.GetComponent<BehaviorManager>())
            {
                point += c.gameObject.GetComponent<Rigidbody>().velocity;
                ++neighborCount;
            }
        }

        if(neighborCount == 0)
        {
            return point;
        }

        point /= neighborCount;

        return point.normalized;
    }

    public Vector3 ComputeCohesion()
    {
        Vector3 point = Vector3.zero;
        int neighborCount = 0;


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, cohesionRange);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject != gameObject && c.gameObject.GetComponent<BehaviorManager>())
            {
                point += c.gameObject.transform.position;
                ++neighborCount;
            }
        }

        if (neighborCount == 0)
        {
            return point;
        }
        
        point /= neighborCount;
        point = point - transform.position;

        return point.normalized;
    }

    public Vector3 ComputeSeparation()
    {
        Vector3 point = Vector3.zero;
        int neighborCount = 0;


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationRange);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject != gameObject && c.gameObject.GetComponent<BehaviorManager>())
            {
                point += c.gameObject.transform.position - transform.position;
                ++neighborCount;
            }
        }
        
        if (neighborCount == 0)
        {
            return point;
        }
        
        point /= neighborCount;
        //point = new Vector3(point.x - transform.position.x, point.y - transform.position.y);
        point *= -1;

        return point.normalized;
    }

}

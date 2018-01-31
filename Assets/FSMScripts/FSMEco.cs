using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EcoState
{
    PURSUE,
    WANDER,
    FLEE
}


public class FSMEco : MonoBehaviour {

    public float speed = 5.0f;

    public float bellySize = 20.0f;
    public float huntingThreshold = 10.0f;
    public float huntingRange = 30.0f;
    public string foodType = "plant";

    public float fleeingRange = 20.0f;
    public string predatorType = "carnivore";

    public float minWanderLength = 2.0f;
    public float maxWanderLength = 5.0f;
    public float wanderArriveThreshold = 0.5f;
    public float wanderTooSlowThreshold = 0.1f;

    public GameObject child;
    public float spawnRate = 10f;
    public int spawnMin = 1;
    public int spawnMax = 3;

    public Rigidbody body;
    


    public EcoState state;
    public float food;
    private Transform target;
    private Vector3 wanderTarg;
    private float spawnTimer = 0;
    private bool resetWander = true;



	// Use this for initialization
	void Start () {
        state = EcoState.WANDER;
        food = bellySize;
        wanderTarg = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        food -= Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            int scount = Random.Range(spawnMin, spawnMax);
            for(int i = 0; i < scount; ++i)
            {
                GameObject kid = Instantiate(child);
                kid.transform.position = transform.position;
                Vector3 posmod = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                kid.transform.position += posmod;
            }
        }

        if(predatorType != "" && FindPredator())
        {
            state = EcoState.FLEE;
        }

		switch(state)
        {
            case EcoState.WANDER:
                Wander();
                if(food < huntingThreshold && FindTarget())
                {
                    state = EcoState.PURSUE;
                }
                break;
            case EcoState.PURSUE:
                if(target == null)
                {
                    SwitchToWander();
                    break;
                }
                Pursue();
                break;
            case EcoState.FLEE:
                if (target == null)
                {
                    SwitchToWander();
                    break;
                }
                Flee();
                if (Vector3.Distance(transform.position, target.position) > fleeingRange)
                {
                    SwitchToWander();
                }
                break;
        }

        if(food <= 0 || transform.position.y < -1f)
        {
            Destroy(gameObject);
        }
	}



    private void SwitchToWander()
    {
        resetWander = true;
        state = EcoState.WANDER;
    }

    private bool FindPredator()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, fleeingRange);

        Transform closest = null;
        float closestdist = float.MaxValue;
        foreach (Collider c in cols)
        {
            if (c.gameObject.tag == predatorType &&
               Vector3.Distance(transform.position, c.transform.position) < closestdist)
            {
                closest = c.transform;
                closestdist = Vector3.Distance(transform.position, c.transform.position);
            }
        }

        if(closest != null)
        {
            target = closest;
        }

        return closest != null;
    }

    private bool FindTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, huntingRange);

        Transform closest = null;
        float closestdist = float.MaxValue;
        foreach(Collider c in cols)
        {
            if(c.gameObject.tag == foodType &&
               c.GetComponent<Eatable>() &&
               Vector3.Distance(transform.position, c.transform.position) < closestdist)
            {
                closest = c.transform;
                closestdist = Vector3.Distance(transform.position, c.transform.position);
            }
        }

        if (closest != null)
        {
            target = closest;
        }

        return closest != null;
    }

    private void Wander()
    {
        if (resetWander || Vector3.Distance(wanderTarg, transform.position) <= wanderArriveThreshold)
        {
            resetWander = false;
            float length = Random.Range(minWanderLength, maxWanderLength);
            wanderTarg = transform.position + body.velocity.normalized * length;

            Vector2 circlepos = Random.insideUnitCircle;
            wanderTarg = new Vector3(wanderTarg.x + circlepos.x, transform.position.y, wanderTarg.z + circlepos.y);
        }

        if (body.velocity.magnitude <= wanderTooSlowThreshold)
        {
            float length = Random.Range(minWanderLength, maxWanderLength);
            wanderTarg = transform.position;

            Vector2 circlepos = Random.insideUnitCircle * length;
            wanderTarg = new Vector3(wanderTarg.x + circlepos.x, transform.position.y, wanderTarg.z + circlepos.y);
        }
        Debug.DrawLine(transform.position, wanderTarg, Color.black);
        Vector3 desiredVelocity = speed * (wanderTarg - transform.position).normalized;

        body.AddForce(desiredVelocity - body.velocity);
    }

    private void Pursue()
    {
        Vector3 projectedPos = transform.position + body.velocity;

        Vector3 desiredVelocity = speed * (target.position - projectedPos).normalized;

        body.AddForce(desiredVelocity - body.velocity);
    }

    private void Flee()
    {
        Vector3 projectedPos = transform.position + body.velocity;

        Vector3 desiredVelocity = -speed * (target.position - projectedPos).normalized;

        body.AddForce(desiredVelocity - body.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == EcoState.PURSUE && collision.gameObject.tag == foodType)
        {
            Destroy(collision.gameObject);
            food = Mathf.Clamp(food + collision.gameObject.GetComponent<Eatable>().foodValue, 0, bellySize);
            SwitchToWander();
        }
    }


}

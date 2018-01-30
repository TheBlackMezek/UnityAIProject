using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner : MonoBehaviour {

    private GoapAction[] actions;
    private Queue<GoapAction> actionQueue;



    private void Start()
    {
        actions = GetComponents<GoapAction>();
    }

    public GoapAction NextAction()
    {
        return actionQueue.Dequeue();
    }

    public void CreateActionQueue(KeyValuePair<string, object> goal)
    {
        List<KeyValuePair<string, object>> open = new List<KeyValuePair<string, object>>();
        List<KeyValuePair<string, object>> closed = new List<KeyValuePair<string, object>>();
        KeyValuePair<string, object> current = new KeyValuePair<string, object>();

        while(!current.Equals(goal))
        {
            //current = node in open with lowest fcost
            //remove current from open
            closed.Add(current);

            if(current.Equals(goal))
            {
                continue;
            }

            //foreach neighbor of current node
                //if neighbor is not traversable or neighbor is closed
                    //skip to next neighboer
                //if new path to neighbor is shorter or neighbor is not in open
                    //set fcost of neighbor
                    //set parent of neighbor to current
                    //if neighbor is not in open
                        //add neighbor to open

        }

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner : MonoBehaviour {

    public Queue<GoapAction> Plan(GameObject agent,
                                  List<GoapAction> availableActions,
                                  List<KeyValuePair<string, object>> worldState,
                                  List<KeyValuePair<string, object>> goal)
    {
        foreach(GoapAction a in availableActions)
        {
            a.DoReset();
        }

        List<GoapAction> usableActions = new List<GoapAction>();
        foreach(GoapAction a in availableActions)
        {
            if(a.CheckProceduralPrecondition(agent))
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();

        Node start = new Node(null, 0, worldState, null);
        bool success = BuildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            Debug.Log("FAILED TO CREATE PLAN");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else if(leaf.runningCost < cheapest.runningCost)
            {
                cheapest = leaf;
            }
        }

        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GoapAction> queue = new Queue<GoapAction>();
        foreach (GoapAction a in result)
        {
            queue.Enqueue(a);
        }

        return queue;
    }

    private bool BuildGraph(Node parent,
                            List<Node> leaves,
                            List<GoapAction> usableActions,
                            List<KeyValuePair<string, object>> goal)
    {
        bool foundOne = false;

        foreach(GoapAction a in usableActions)
        {
            if(InState(a.Preconditions, parent.state))
            {
                List<KeyValuePair<string, object>> currentState = PopulateState(parent.state,
                                                                                a.Effects);
                Node node = new Node(parent, parent.runningCost + a.cost, currentState, a);

                if(InState(goal, currentState))
                {
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    List<GoapAction> subset = ActionSubset(usableActions, a);

                    bool found = BuildGraph(node, leaves, subset, goal);
                    if(found)
                    {
                        foundOne = true;
                    }
                }
            }
        }

        return foundOne;
    }

    private bool InState(List<KeyValuePair<string, object>> test,
                         List<KeyValuePair<string, object>> state)
    {
        //bool allMatch = true;
        foreach(KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach(KeyValuePair<string, object> s in state)
            {
                if(s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if(!match)
            {
                //allMatch = false;
                return false;
                //break;
            }
        }
        //return allMatch;
        return true;
    }

    private List<KeyValuePair<string, object>> PopulateState(List<KeyValuePair<string, object>> currentState,
                                                             List<KeyValuePair<string, object>> stateChange)
    {
        List<KeyValuePair<string, object>> state = new List<KeyValuePair<string, object>>();

        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach(KeyValuePair<string, object> change in stateChange)
        {
            bool exists = false;

            foreach(KeyValuePair<string, object> s in state)
            {
                if(s.Equals(change))
                {
                    exists = true;
                    break;
                }
            }

            if(exists)
            {
                foreach(KeyValuePair<string, object> kvp in state)
                {
                    if(kvp.Key.Equals(change.Key))
                    {
                        state.Remove(kvp);
                    }
                }
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key,
                                                                                        change.Value);
                state.Add(updated);
            }
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }

        return state;
    }


    private List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction removeMe)
    {
        List<GoapAction> subset = new List<GoapAction>();
        foreach(GoapAction a in actions)
        {
            if(!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }



    private class Node
    {
        public Node parent;
        public float runningCost;
        public List<KeyValuePair<string, object>> state;
        public GoapAction action;

        public Node(Node parent,
                    float runningCost,
                    List<KeyValuePair<string, object>> state,
                    GoapAction action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }

}

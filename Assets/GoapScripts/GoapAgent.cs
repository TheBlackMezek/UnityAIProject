using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour {

    public NavMeshAgent agent;
    public Transform target;

    private FSM fsm;
    private GoapPlanner planner;

    private FSM.FSMState idleState;
    private FSM.FSMState moveToState;
    private FSM.FSMState actionState;

    private List<GoapAction> availableActions;
    private Queue<GoapAction> actionQueue;

    private IGoap dataProvider;




    private void Start()
    {
        fsm = new FSM();
        planner = new GoapPlanner();

        availableActions = new List<GoapAction>();
        actionQueue = new Queue<GoapAction>();

        FindDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreateActionState();

        fsm.PushState(idleState);

        LoadActions();
    }

    void Update () {
        agent.SetDestination(target.position);
	}

    

    public void AddAction(GoapAction a)
    {
        availableActions.Add(a);
    }

    public void RemoveAction(GoapAction a)
    {
        availableActions.Remove(a);
    }

    public GoapAction GetAction(System.Type action)
    {
        foreach(GoapAction g in availableActions)
        {
            if(g.GetType().Equals(action))
            {
                return g;
            }
        }
        return null;
    }



    private bool HasActionPlan()
    {
        return actionQueue.Count > 0;
    }

    private void CreateIdleState()
    {
        idleState = (fsm, obj) =>
        {
            List<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
            List<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

            Queue<GoapAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                actionQueue = plan;
                dataProvider.PlanFound(goal, plan);

                fsm.PopState();
                fsm.PushState(actionState);
            }
            else
            {
                Debug.Log("<color=orange>Failed Plan:</color>" + PrettyPrint(goal));
                dataProvider.PlanFailed(goal);

                fsm.PopState();
                fsm.PushState(idleState);
            }
        };
    }

    private void CreateMoveToState()
    {
        moveToState = (fsm, obj) =>
        {
            GoapAction action = actionQueue.Peek();
            if (action.RequiresInRange() && action.target == null)
            {
                Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none." +
                    "Planning failed. You did not assign the target in your" +
                    "Action.checkProceduralPrecondition()");
                fsm.PopState(); //move
                fsm.PopState(); //action
                fsm.PushState(idleState);
                return;
            }

            if (dataProvider.MoveAgent(action))
            {
                fsm.PopState();
            }
        };
    }

    private void CreateActionState()
    {
        actionState = (fsm, obj) =>
        {
            if(!HasActionPlan())
            {
                Debug.Log("<color=red>Done actions</color>");
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
                return;
            }

            GoapAction action = actionQueue.Peek();
            if(action.IsDone())
            {
                actionQueue.Dequeue();
            }

            if(HasActionPlan())
            {
                action = actionQueue.Peek();
                bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                if(inRange)
                {
                    bool success = action.Perform(obj);

                    if(!success)
                    {
                        fsm.PopState();
                        fsm.PushState(idleState);
                        dataProvider.PlanAborted(action);
                    }
                }
                else
                {
                    fsm.PushState(moveToState);
                }
            }
            else
            {
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
            }
        };
    }

    private void FindDataProvider()
    {
        //foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        //{
        //    if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
        //    {
        //        dataProvider = (IGoap)comp;
        //        return;
        //    }
        //}

        foreach(Component comp in gameObject.GetComponents<Component>())
        {
            if(typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }

    private void LoadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction a in actions)
        {
            availableActions.Add(a);
        }
        Debug.Log("Found actions: " + PrettyPrint(actions));
    }




    public static string PrettyPrint(List<KeyValuePair<string, object>> state)
    {
        System.String s = "";
        foreach (KeyValuePair<string, object> kvp in state)
        {
            s += kvp.Key + ":" + kvp.Value.ToString();
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(Queue<GoapAction> actions)
    {
        System.String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }

    public static string PrettyPrint(GoapAction[] actions)
    {
        System.String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(GoapAction action)
    {
        System.String s = "" + action.GetType().Name;
        return s;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copied from https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793

public abstract class GoapAction : MonoBehaviour {

    private HashSet<KeyValuePair<string, object>> preconditions;
    private HashSet<KeyValuePair<string, object>> effects;

    private bool inRange = false;

    public float cost = 1f;
    public GameObject target = null;



    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();
    }

    public void DoReset()
    {
        inRange = false;
        target = null;
        ResetAction();
    }

    public abstract void ResetAction();

    public abstract bool IsDone();

    public abstract bool CheckProceduralPrecondition(GameObject agent);

    public abstract bool Perform(GameObject agent);

    public abstract bool RequiresInRange();



    public bool IsInRange()
    {
        return inRange;
    }

    public void SetInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    public void AddPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach(KeyValuePair<string, object> kvp in preconditions)
        {
            if(kvp.Key.Equals(key))
            {
                remove = kvp;
            }
        }
        if(!default(KeyValuePair<string, object>).Equals(remove))
        {
            preconditions.Remove(remove);
        }
    }

    public void AddEffect(string key, object value)
    {
        effects.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemoveEffect(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            if (kvp.Key.Equals(key))
            {
                remove = kvp;
            }
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            effects.Remove(remove);
        }
    }

    public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get
        {
            return preconditions;
        }
    }

    public HashSet<KeyValuePair<string, object>> Effects
    {
        get
        {
            return effects;
        }
    }

}

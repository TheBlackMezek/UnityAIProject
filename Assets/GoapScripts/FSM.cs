using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum GoapState
{
    IDLE,
    MOVETO,
    ACTION
}


public class FSM : MonoBehaviour {

    
    private Stack<FSMState> stateStack = new Stack<FSMState>();

    public delegate void FSMState(FSM fsm, GameObject obj);



	
	void Update () {
		if(stateStack.Peek() != null)
        {
            stateStack.Peek().Invoke(this, gameObject);
        }
	}

    public void PushState(FSMState state)
    {
        stateStack.Push(state);
    }

    public void PopState()
    {
        stateStack.Pop();
    }
}

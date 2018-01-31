using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FSMState {
    
    void Update(FSM fsm, GameObject obj);
    void Updadte(FSM fsm, GameObject obj);

}

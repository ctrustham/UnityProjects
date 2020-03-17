using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    // works in tandem with stateManager
    public class State
    {
        //because turn-based game, only actions on update
        public List<StateActions> actions = new List<StateActions>();

        //need stateManager, sessionManager and turn to know who is running the Tick
        public void Tick(StateManager states, SessionManager sm, Turn t)
        {
            if (states.forceExit)
            {
                return;
            }
            for (int i = 0; i<actions.Count; i++)
			{
                actions[i].Execute(states, sm, t);
			}
        }
    }
}

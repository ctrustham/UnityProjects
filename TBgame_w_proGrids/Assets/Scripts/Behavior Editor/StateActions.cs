using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    /// <summary>
    /// An abstract class for executing in-game actions
    /// </summary>
    public abstract class StateActions // if wanted to use node-action (SharpAccent GitHub) then derive from scriptable obj (and non abstract?)
    {
        /// <summary>
        /// Executes in-game actions
        /// </summary>
        /// <param name="sm"> The Session Manager</param>
        public abstract void Execute(StateManager states, SessionManager sm, Turn t);

    }
}
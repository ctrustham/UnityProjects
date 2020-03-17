using UnityEngine;
using System.Collections;

namespace SA
{
    public abstract class Phase : ScriptableObject
    {
        public string phaseName;
        //[System.NonSerialized]
        public bool forceExit;

        public abstract bool isComplete(SessionManager sm, Turn turn);

        [System.NonSerialized]
        protected bool isInit;

        public abstract void OnStartPhase(SessionManager sm, Turn turn);

        public virtual void OnEndPhase(SessionManager sm, Turn turn)
        {
            isInit = false;
            forceExit = false;
        }
    }

}

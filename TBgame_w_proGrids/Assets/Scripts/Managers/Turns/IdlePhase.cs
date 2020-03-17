using UnityEngine;
using System.Collections;

namespace SA
{
    [CreateAssetMenu(menuName = "Phases/Idle Phase")] //needed for scriptable objects
    public class IdlePhase : Phase
    {
        public override bool isComplete(SessionManager sm, Turn turn)
        {
           
            return false; //endless phase
        }

        public override void OnStartPhase(SessionManager sm, Turn turn)
        {
            if (isInit)
                return;
            isInit = true;

            Debug.Log("Idle Phase Started"); 
        }

        public override void OnEndPhase(SessionManager sm, Turn turn)
        {

        }

    }
}
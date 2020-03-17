using UnityEngine;
using System.Collections;

namespace SA
{
    [CreateAssetMenu(menuName = "Game/Turn")]
    public class Turn : ScriptableObject
    {
        public PlayerHolder player;
        [System.NonSerialized]
        int phaseIndex = 0;
        public Phase[] phases;

        public bool Execute(SessionManager sm)
        {
            bool result = false;

            phases[phaseIndex].OnStartPhase(sm, this);

            if(phases[phaseIndex].isComplete(sm, this))
            {
                phases[phaseIndex].OnEndPhase(sm, this);
                phaseIndex++;

                if(phaseIndex > phases.Length - 1)
                {
                    phaseIndex = 0;
                    result = true;
                }
            }

            return result;
        }

        public void EndCurentPhase()
        {
            phases[phaseIndex].forceExit = true;
        }
    }
}
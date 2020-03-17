using UnityEngine;
using System.Collections;
namespace SA
{
    [CreateAssetMenu(menuName ="Phases/States Tick")]
    public class StateTickPhase : Phase
    {
        public override bool isComplete(SessionManager sm, Turn turn)
        {
            turn.player.stateManager.Tick(sm, turn);
            return false;
        }

        public override void OnStartPhase(SessionManager sm, Turn turn)
        {
            
        }
    }
}

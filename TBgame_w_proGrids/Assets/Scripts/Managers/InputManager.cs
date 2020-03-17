using UnityEngine;
using System.Collections;
namespace SA
{
    public class InputManager : StateActions
    {
        VariablesHolder varHolder;
        public InputManager(VariablesHolder vh)
        {
            varHolder = vh;
        }
        public override void Execute(StateManager states, SessionManager sm, Turn t)
        {
            varHolder.horizontalInput.value = Input.GetAxis("Horizontal");
            varHolder.verticalInput.value = Input.GetAxis("Vertical");
        }
    }
}
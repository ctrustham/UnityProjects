using UnityEngine;
using System.Collections;

namespace SA
{
    public class MoveCameraTransform : StateActions
    {
        TransformVariable cameraTransform;
        FloatVariable horizontal;
        FloatVariable vertical;

        VariablesHolder varHolder;

        // allows for assigning variables (like above) without making this a scriptable object
        public MoveCameraTransform(VariablesHolder vh)
        {
            varHolder = vh;
            cameraTransform = varHolder.cameraTransform;
            horizontal = varHolder.horizontalInput;
            vertical = varHolder.verticalInput;
        }

        public override void Execute(StateManager states, SessionManager sm, Turn t)
        {
            Vector3 targetPos = cameraTransform.value.forward * (vertical.value * varHolder.CameraMoveSpeed * states.delta);
            targetPos += cameraTransform.value.right * (horizontal.value * varHolder.CameraMoveSpeed * states.delta);

            cameraTransform.value.position += targetPos ;
        }
    }
}
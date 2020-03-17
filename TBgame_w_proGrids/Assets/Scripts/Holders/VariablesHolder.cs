using UnityEngine;
using System.Collections;

namespace SA {
    [CreateAssetMenu(menuName ="Game Variables Holder")]
    public class VariablesHolder : ScriptableObject
    {
        public float CameraMoveSpeed =15; 

        [Header("Scriptable Variables")]
        #region Scriptables
        public TransformVariable cameraTransform;
        public FloatVariable horizontalInput;
        public FloatVariable verticalInput;
        #endregion
    }
}

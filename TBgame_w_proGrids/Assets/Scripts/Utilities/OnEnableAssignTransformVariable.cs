using UnityEngine;
using System.Collections;

namespace SA
{
    public class OnEnableAssignTransformVariable : MonoBehaviour
    {
        public TransformVariable targetVariable;

        private void Awake()
        {
            targetVariable.value = this.transform;
            Destroy(this);
        }
    }
}

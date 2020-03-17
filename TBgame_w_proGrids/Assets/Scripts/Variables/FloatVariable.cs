using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    [CreateAssetMenu(menuName ="Variables/Float")]
    public class FloatVariable : ScriptableObject
    {
        public float value;

        /*
        public FloatVariable(float v)
        {
            value += v;
        }
   
        
        //define how to add floatVariables
        public static FloatVariable operator +(FloatVariable v)
        {
            return new FloatVariable(v.value);
        }
        */
    }
}

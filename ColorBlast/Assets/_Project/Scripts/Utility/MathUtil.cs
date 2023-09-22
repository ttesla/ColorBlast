using UnityEngine;

namespace ColorBlast
{
    public class MathUtil
    {
        public static float Remap(float aLow, float aHigh, float bLow, float bHigh, float value) 
        {
            float normal = Mathf.InverseLerp(aLow, aHigh, value);
            float result = Mathf.Lerp(bLow, bHigh, normal);

            return result;
        }    
    }
}

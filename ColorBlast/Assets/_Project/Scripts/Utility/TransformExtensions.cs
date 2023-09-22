using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class TransformExtensions
    {
        public static void DeleteChildren(this Transform input)
        {
            for (int i = 0; i < input.childCount; i++)
            {
                GameObject.Destroy(input.GetChild(i).gameObject);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "ScriptableObjects/ConfigData", order = 1)]
    public class ConfigData : ScriptableObject
    {
        public ApplicationSettings AppSettings;
        public DoTweenSettings TweenSettings;

    }

    [Serializable]
    public class DoTweenSettings 
    {
        public int TweenCapacity;
        public int SequenceCapacity;
    }

    [Serializable]
    public class ApplicationSettings
    {
        public int TargetFrameRate;
    }
}

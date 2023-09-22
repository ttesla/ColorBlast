using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public enum SfxType
    {
        None = 0,
        ButtonClick,
        Pop,
        BoardShuffle,
        BoosterBomb,
        BoosterRocket,
        BoosterReveral,
        LevelComplete,
        Success
    }

    [Serializable]
    public class AudioSfx
    {
        public SfxType Type;

        [Range(0.0f, 1.0f)]
        public float VolumeScale;
        public AudioClip[] Clips;

        // There may be several versions of the same Sfx type, this allows us to play randomly
        // Also works for single clips. 
        public AudioClip RandomClip => Clips[UnityEngine.Random.Range(0, Clips.Length)];
    }

    [CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData", order = 1)]
    public class AudioData : ScriptableObject
    {
        public List<AudioSfx> SfxList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class AudioService : IAudioService
    {
        public void Init()
        {
            Debug.Log("AudioService - Init");
        }

        public void Release()
        {
            Debug.Log("AudioService - Release");
        }

        public void Play(AudioClip audioClip)
        {
            Debug.Log("AudioService - Play");
        }

        public void Play(AudioSource audioSource, AudioClip audioClip)
        {
            Debug.Log("AudioService - Play with AudioSource");
        }
    }
}

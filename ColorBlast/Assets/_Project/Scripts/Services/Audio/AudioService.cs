using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class AudioService : IAudioService
    {
        public void Init()
        {
            Logman.Log("AudioService - Init");
        }

        public void Release()
        {
            Logman.Log("AudioService - Release");
        }

        public void Play(AudioClip audioClip)
        {
            
        }

        public void Play(AudioSource audioSource, AudioClip audioClip)
        {
            
        }
    }
}

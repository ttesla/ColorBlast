using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class DummyAudioService : IAudioService
    {
        public void Init()
        {
            Debug.Log("DummyAudioService - Init");
        }

        public void Release()
        {
            Debug.Log("DummyAudioService - Release");
        }

        public void Play(AudioClip audioClip)
        {
            Debug.Log("DummyAudioService - Play AudioClip: " + audioClip.name);
        }

        public void Play(AudioSource audioSource, AudioClip audioClip)
        {
            Debug.Log("DummyAudioService - Play AudioSource: " + audioSource.name + ", AudioClip: " + audioClip.name);
        }
    }
}
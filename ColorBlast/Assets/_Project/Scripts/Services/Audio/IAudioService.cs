using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface IAudioService : IService
    {
        void Play(AudioClip audioClip);
        void Play(AudioSource audioSource, AudioClip audioClip);
    }
}

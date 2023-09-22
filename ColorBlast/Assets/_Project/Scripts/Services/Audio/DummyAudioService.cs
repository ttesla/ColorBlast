using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// Dummy Audio Service, added for testing the ServiceManager. 
    /// Left in the project as an example for making a different Audio Service implementation. 
    /// </summary>
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

        public void PlaySfx(SfxType sfxType)
        {
            Debug.Log("DummyAudioService - PlaySfx: " + sfxType);
        }

        public void SetEnableSfx(bool enabled)
        {
            Debug.Log("DummyAudioService - SetEnableSfx: " + enabled);
        }

        public void SetMute(bool isMute)
        {
            Debug.Log("DummyAudioService - SetMute: " + isMute);
        }
    }
}

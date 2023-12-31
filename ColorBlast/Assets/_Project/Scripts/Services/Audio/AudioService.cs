using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Utility;

namespace ColorBlast
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioMixer Mixer;
        [SerializeField] private AudioSource SfxSource;
        [SerializeField] private AudioData AudioData;

        private Dictionary<SfxType, AudioSfx> mSfxDict;

        private ISettingsService mSettingsService;

        public void Init()
        {
            FillSfxDictionary();

            mSettingsService = ServiceManager.Instance.Get<ISettingsService>();
            SetEnableSfx(mSettingsService.GetAudioSettings().SfxIsOn);
            mSettingsService.SettingsUpdated += OnSettinsUpdated;

            Logman.Log("AudioService - Init");
        }

        public void Release()
        {
            mSettingsService.SettingsUpdated -= OnSettinsUpdated;
            Logman.Log("AudioService - Release");
        }

        private void OnSettinsUpdated()
        {
            SetEnableSfx(mSettingsService.GetAudioSettings().SfxIsOn);
        }

        private void FillSfxDictionary() 
        {
            mSfxDict = new Dictionary<SfxType, AudioSfx>();

            foreach(var sfx in AudioData.SfxList) 
            {
                mSfxDict.Add(sfx.Type, sfx);
            }
        }

        public void PlaySfx(SfxType sfxType)
        {
            if(mSfxDict.TryGetValue(sfxType, out AudioSfx sfx)) 
            {
                SfxSource.PlayOneShot(sfx.RandomClip, sfx.VolumeScale);
            }
        }

        public void SetEnableSfx(bool enabled)
        {
            SfxSource.mute = !enabled;

            float volume = enabled ? 0.0f : -80.0f;
            Mixer.SetFloat("SfxVol", volume);
        }

        public void SetMute(bool isMute)
        {
            float volume = isMute ? -80.0f : 0.0f;
            Mixer.SetFloat("MasterVol", volume);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public struct AudioSetting 
    {
        public bool SfxIsOn;
    }

    public class SettingsService : ISettingsService
    {
        public event Action SettingsUpdated;

        private ISaveService mSaveService;

        public void Init()
        {
            mSaveService = ServiceManager.Instance.Get<ISaveService>();
            Logman.Log("SettingService - Init");
        }

        public void Release()
        {
            Logman.Log("SettingService - Release");
        }

        public void SetAudioSettings(AudioSetting audioSetting)
        {
            var saveData = mSaveService.GetSaveData();
            saveData.Settings.SfxIsOn = audioSetting.SfxIsOn;
            mSaveService.Save();

            SettingsUpdated?.Invoke();
        }

        public AudioSetting GetAudioSettings()
        {
            var saveData = mSaveService.GetSaveData();
            AudioSetting audioSetting = new AudioSetting() 
            {
                SfxIsOn = saveData.Settings.SfxIsOn,
            };

            return audioSetting;
        }
    }
}

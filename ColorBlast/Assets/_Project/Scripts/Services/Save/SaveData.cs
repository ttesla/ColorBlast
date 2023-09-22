using System.Collections.Generic;
using System;
using UnityEngine;

namespace ColorBlast
{
    [Serializable]
    public class SaveData
    {
        public SettingsData Settings;

        public SaveData()
        {

        }

        public void ResetToDefaults()
        {
            Settings        = new SettingsData();
        }
    }

    #region DataDefinitions

    [Serializable]
    public class SettingsData
    {
        public bool SfxIsOn;

        public SettingsData()
        {
            SfxIsOn = true;
        }

        public void SetSfxIsOn(bool isOn)
        {
            SfxIsOn = isOn;
        }
    }

    #endregion
}

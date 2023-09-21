using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class SettingsService : ISettingsService
    {
        public void Init()
        {
            Logman.Log("SettingService - Init");
        }

        public void Release()
        {
            Logman.Log("SettingService - Release");
        }

        public void Apply()
        {

        }
    }
}

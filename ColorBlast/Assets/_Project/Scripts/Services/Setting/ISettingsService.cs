


using System;

namespace ColorBlast
{
    public interface ISettingsService : IService
    {
        event Action SettingsUpdated;
        void SetAudioSettings(AudioSetting audioSetting);
        AudioSetting GetAudioSettings();
    }
}

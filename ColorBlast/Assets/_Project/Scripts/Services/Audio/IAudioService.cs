
namespace ColorBlast
{
    public interface IAudioService : IService
    {
        public void PlaySfx(SfxType sfxType);
        public void SetEnableSfx(bool enabled);
        public void SetMute(bool isMute);
    }
}

using System;
using Utility;

namespace ColorBlast
{
    public class GameService : IGameService
    {
        public event Action GameInited;
        public event Action SessionStarted;
        public event Action SessionEnded;

        private IAudioService mAudioService;
        private ILevelService mLevelService;

        public void Init()
        {
            mAudioService = ServiceManager.Instance.Get<IAudioService>();
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
            mLevelService.LevelCompleted += OnLevelCompleted;
            
            GameInited?.Invoke();

            Logman.Log("GameService - Init");
        }

        public void Release()
        {
            mLevelService.LevelCompleted -= OnLevelCompleted;
            Logman.Log("GameService - Release");
        }

        public void StartSession()
        {
            SessionStarted?.Invoke();
        }

        public void EndSession()
        {
            SessionEnded?.Invoke(); 
        }

        private void OnLevelCompleted()
        {
            mAudioService.PlaySfx(SfxType.LevelComplete);
            EndSession();
        }
    }
}

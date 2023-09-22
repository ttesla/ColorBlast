using System;
using Utility;

namespace ColorBlast
{
    public class GameService : IGameService
    {
        public event Action GameInited;
        public event Action SessionStarted;
        public event Action SessionEnded;

        public void Init()
        {
            GameInited?.Invoke();
            Logman.Log("GameService - Init");
        }

        public void Release()
        {
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
    }
}

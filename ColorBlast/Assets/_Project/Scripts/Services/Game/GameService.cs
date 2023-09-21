using System;
using Utility;

namespace ColorBlast
{
    public struct SessionParameters 
    {
        public int Width;
        public int Height;
    }

    public class GameService : IGameService
    {
        public event Action<SessionParameters> SessionStarted;
        public event Action SessionEnded;

        public void Init()
        {
            Logman.Log("GameService - Init");
        }

        public void Release()
        {
            Logman.Log("GameService - Release");
        }

        public void StartSession(int width, int height)
        {
            SessionStarted?.Invoke(new SessionParameters() 
            {
                Width = width,
                Height = height
            });
        }

        public void EndSession()
        {
            SessionEnded?.Invoke(); 
        }
    }
}


using System;

namespace ColorBlast
{
    public interface IGameService : IService
    {
        event Action<SessionParameters> SessionStarted;
        event Action SessionEnded;

        void StartSession(int width, int height);
        void EndSession();
    }
}

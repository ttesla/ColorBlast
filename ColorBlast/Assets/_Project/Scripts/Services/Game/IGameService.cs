
using System;

namespace ColorBlast
{
    public interface IGameService : IService
    {
        event Action SessionStarted;
        event Action SessionEnded;

        void StartSession();
        void EndSession();
    }
}

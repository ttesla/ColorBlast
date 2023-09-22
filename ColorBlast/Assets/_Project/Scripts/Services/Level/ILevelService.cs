using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface ILevelService : IService
    {
        event Action<Level> LevelLoaded;
        event Action LevelCompleted;

        void LoadLevel(int levelIndex);
        int GetTotalLevelCount();
        void NotifyTilePop(TileType tileType, int count);
    }
}

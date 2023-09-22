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
        event Action<List<LevelGoal>> LevelGoalUpdated;

        void LoadLevel(int levelIndex);
        int GetTotalLevelCount();
        void NotifyTilePop(TileType tileType, int count);
        List<Level> GetLevelList();
        List<LevelGoal> GetLevelGoalList();
        Level GetCurrentLevel();
        int GetLastLoadedLevelIndex();
    }
}

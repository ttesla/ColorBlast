using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class LevelService : ILevelService
    {
        public event Action<Level> LevelLoaded;
        public event Action LevelCompleted;

        private LevelData mLevelData;

        private List<LevelGoal> mCurrentGoals;

        public LevelService(LevelData levelData) 
        {
            mLevelData = levelData;
        }

        public void Init()
        {
            Logman.Log("LevelService - Init");
        }

        public void Release()
        {
            Logman.Log("LevelService - Release");
        }

        public int GetTotalLevelCount()
        {
            return mLevelData.LevelList.Count;
        }

        public void LoadLevel(int levelIndex)
        {
            if(levelIndex < GetTotalLevelCount()) 
            {
                var level = mLevelData.LevelList[levelIndex];
                CopyCurrentGoals(level);

                LevelLoaded?.Invoke(level);
            }
            else 
            {
                Debug.LogError("Invalid level index: " + levelIndex);
            }
        }

        public void NotifyTilePop(TileType tileType, int count)
        {
            var goal = mCurrentGoals.Find(x => x.TargetTile == tileType);

            if(goal != null && goal.Count > 0) 
            {
                goal.Count -= count;

                if(goal.Count < 0) 
                {
                    goal.Count = 0;
                }
            }

            // Check level complete
            if (CheckAllGoalsAreCompleted()) 
            {
                LevelCompleted?.Invoke();
            }
        }

        private void CopyCurrentGoals(Level level) 
        {
            mCurrentGoals = new List<LevelGoal>();

            foreach (var goal in level.Goals) 
            {
                mCurrentGoals.Add(new LevelGoal() 
                {
                    TargetTile = goal.TargetTile,
                    Count      = goal.Count
                });
            }
        }

        private bool CheckAllGoalsAreCompleted() 
        {
            foreach(var goal in mCurrentGoals) 
            {
                if(goal.Count > 0) 
                {
                    return false;
                }
            }

            // If we are up to here, all goals are completed
            return true;
        }
    }
}

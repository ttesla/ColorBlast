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
        public event Action<List<LevelGoal>> LevelGoalUpdated;

        private LevelData mLevelData;
        private List<LevelGoal> mCurrentGoals;
        private Level mCurrentLevel;
        private int mLastLoadedLevelIndex;

        private bool mLevelCompleted;

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
            mLastLoadedLevelIndex = levelIndex;
            
            if (mLastLoadedLevelIndex < GetTotalLevelCount()) 
            {
                mCurrentLevel = mLevelData.LevelList[mLastLoadedLevelIndex];
                CopyCurrentGoals(mCurrentLevel);
                mLevelCompleted = false;

                LevelLoaded?.Invoke(mCurrentLevel);
            }
            else 
            {
                Debug.LogError("Invalid level index: " + mLastLoadedLevelIndex);
            }
        }

        public List<Level> GetLevelList()
        {
            return mLevelData.LevelList;
        }

        public List<LevelGoal> GetLevelGoalList() 
        {
            return mCurrentGoals;
        }

        public Level GetCurrentLevel()
        {
            return mCurrentLevel;
        }

        public int GetLastLoadedLevelIndex() 
        {
            return mLastLoadedLevelIndex;
        }

        public void NotifyTilePop(TileType tileType, int count)
        {
            // If level is completed, make early return
            if(mLevelCompleted) 
            {
                return;
            }

            var goal = mCurrentGoals.Find(x => x.TargetTile == tileType);

            if(goal != null && goal.Count > 0) 
            {
                goal.Count -= count;

                if(goal.Count < 0) 
                {
                    goal.Count = 0;
                }
            }

            // Broadcast latest goal stats
            LevelGoalUpdated?.Invoke(mCurrentGoals);

            // Check level complete
            if (CheckAllGoalsAreCompleted()) 
            {
                mLevelCompleted = true;
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

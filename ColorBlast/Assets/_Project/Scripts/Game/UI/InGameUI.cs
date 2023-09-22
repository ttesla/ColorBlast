using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class InGameUI : MonoBehaviour, IWindow
    {
        [SerializeField] private GoalItemUI GoalItemUIPrefab;
        [SerializeField] private Transform GoalItemContainerTransform;

        private IGameService mGameService;
        private ILevelService mLevelService;
        private Dictionary<TileType, GoalItemUI> mGoalItems;

        void Awake() 
        {
            mGameService  = ServiceManager.Instance.Get<IGameService>();
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
            mGoalItems    = new Dictionary<TileType, GoalItemUI>();
        }

        void OnEnable()
        {
            mGameService.SessionStarted    += OnSessionStarted;
            mLevelService.LevelGoalUpdated += OnLevelGoalUpdated;
        }

        void OnDisable() 
        {
            mGameService.SessionStarted    -= OnSessionStarted;
            mLevelService.LevelGoalUpdated -= OnLevelGoalUpdated;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnSessionStarted()
        {
            PopulateLevelGoals();
        }

        private void PopulateLevelGoals() 
        {
            // Clear it first 
            // TODO: Pool or cache the items
            GoalItemContainerTransform.DeleteChildren();
            mGoalItems.Clear();

            var levelGoals = mLevelService.GetLevelGoalList();

            foreach(var levelGoal in levelGoals) 
            {
                var goalItemUI = GameObject.Instantiate(GoalItemUIPrefab, GoalItemContainerTransform);
                goalItemUI.SetGoal(levelGoal);

                // Add them to dictionary to update them when goals are changed
                mGoalItems.Add(levelGoal.TargetTile, goalItemUI);
            }
        }

        private void OnLevelGoalUpdated(List<LevelGoal> levelGoals) 
        {
            foreach(var levelGoal in levelGoals) 
            {
                if (mGoalItems.TryGetValue(levelGoal.TargetTile, out GoalItemUI goalItemUI))
                {
                    goalItemUI.UpdateCount(levelGoal.Count);
                }
            }
        }
    }
}

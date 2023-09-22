using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ColorBlast
{
    public class LevelUI : MonoBehaviour, IWindow
    {
        [SerializeField] private LevelItemUI LevelItemUIPrefab;
        [SerializeField] private Transform ItemContainerTransform;

        private ILevelService mLevelService;
     
        private bool mListPopulated = false;

        void Awake() 
        {
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
        }

        void OnEnable() 
        {
            mLevelService.LevelLoaded += OnLevelLoaded;
        }

        void OnDisable() 
        {
            mLevelService.LevelLoaded -= OnLevelLoaded;
        }
        
        public void Open()
        {
            gameObject.SetActive(true);

            // If the list is populated before, skip
            if (!mListPopulated) 
            {
                PopulateList();
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnLevelLoaded(Level obj)
        {
            // Close level window if a level is loaded.
            Close();
        }

        private void PopulateList() 
        {
            var levels = mLevelService.GetLevelList();

            for(int i = 0; i < levels.Count; i++) 
            {
                var levelItemUI = GameObject.Instantiate(LevelItemUIPrefab, ItemContainerTransform)
                                            .GetComponent<LevelItemUI>();

                levelItemUI.Fill(i + 1, levels[i]);
            }

            mListPopulated = true;
        }
    }
}

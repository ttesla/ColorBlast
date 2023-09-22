using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class MainUI : MonoBehaviour, IWindow
    {
        [Header("Other Windows")]
        [SerializeField] private LevelUI LevelUI;

        [Header("Buttons")]
        [SerializeField] private Button HomeButton;
        [SerializeField] private Button MuteButton;
        [SerializeField] private Button PlayButton;

        private ILevelService mLevelService;

        void Awake()
        {
            HomeButton.onClick.AddListener(OnHomeClicked);
            MuteButton.onClick.AddListener(OnMuteClicked);
            PlayButton.onClick.AddListener(OnPlayClicked);

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

        private void OnLevelLoaded(Level obj)
        {
            // Enable play button
            PlayButton.gameObject.SetActive(true);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(true);
        }

        #region Button Callbacks

        private void OnHomeClicked() 
        {
            ServiceManager.Instance.Get<IGameService>().EndSession();
            LevelUI.Open();
        }

        private void OnMuteClicked() 
        {

        }

        private void OnPlayClicked()
        {
            ServiceManager.Instance.Get<IGameService>().StartSession();
            PlayButton.gameObject.SetActive(false);
            LevelUI.Close();
        }

        #endregion
    }
}

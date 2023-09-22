using DG.Tweening;
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
        [SerializeField] private InGameUI InGameUI;

        [Header("Buttons")]
        [SerializeField] private Button HomeButton;
        [SerializeField] private Button MuteButton;
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button ReplayButton;

        private ILevelService mLevelService;
        private IGameService mGameService;

        void Awake()
        {
            HomeButton.onClick.AddListener(OnHomeClicked);
            MuteButton.onClick.AddListener(OnMuteClicked);
            PlayButton.onClick.AddListener(OnPlayClicked);
            ReplayButton.onClick.AddListener(OnReplayClicked);

            mLevelService = ServiceManager.Instance.Get<ILevelService>();
            mGameService = ServiceManager.Instance.Get<IGameService>();
        }

        void OnEnable() 
        {
            mLevelService.LevelLoaded    += OnLevelLoaded;
            mLevelService.LevelCompleted += OnLevelCompleted;
            mGameService.GameInited      += OnGameInited;

        }

        void OnDisable() 
        {
            mLevelService.LevelLoaded    -= OnLevelLoaded;
            mLevelService.LevelCompleted -= OnLevelCompleted;
            mGameService.GameInited      -= OnGameInited;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(true);
        }

        private void OnLevelLoaded(Level obj)
        {
            // Enable play button
            PlayButton.gameObject.SetActive(true);
        }

        private void OnLevelCompleted()
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                // Level completed enable replay button with small delay
                ReplayButton.gameObject.SetActive(true);
            });
        }

        private void OnGameInited()
        {
            LevelUI.Open();
        }

        #region Button Callbacks

        private void OnHomeClicked() 
        {
            LevelUI.Open();
            InGameUI.Close();

            ServiceManager.Instance.Get<IGameService>().EndSession();
            PlayButton.gameObject.SetActive(false);
            ReplayButton.gameObject.SetActive(false);
        }

        private void OnMuteClicked() 
        {

        }

        private void OnPlayClicked()
        {
            LevelUI.Close();
            InGameUI.Open();
            PlayButton.gameObject.SetActive(false);
         
            ServiceManager.Instance.Get<IGameService>().StartSession();
        }

        private void OnReplayClicked() 
        {
            // Reload current level
            var lastIndex = mLevelService.GetLastLoadedLevelIndex();
            mLevelService.LoadLevel(lastIndex);
            ReplayButton.gameObject.SetActive(false);
        }

        #endregion
    }
}

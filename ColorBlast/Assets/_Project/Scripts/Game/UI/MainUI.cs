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

        [Header("Sprite Assets")]
        [SerializeField] private Image ReplayButtonImg;
        [SerializeField] private Sprite SfxIsOnSprite;
        [SerializeField] private Sprite SfxIsOffSprite;

        private ILevelService mLevelService;
        private IGameService mGameService;
        private ISettingsService mSettingsService;

        void Awake()
        {
            HomeButton.onClick.AddListener(OnHomeClicked);
            MuteButton.onClick.AddListener(OnMuteClicked);
            PlayButton.onClick.AddListener(OnPlayClicked);
            ReplayButton.onClick.AddListener(OnReplayClicked);

            mLevelService    = ServiceManager.Instance.Get<ILevelService>();
            mGameService     = ServiceManager.Instance.Get<IGameService>();
            mSettingsService = ServiceManager.Instance.Get<ISettingsService>();
        }

        void Start() 
        {
            UpdateSfxSetting();
        }

        void OnEnable() 
        {
            mLevelService.LevelLoaded        += OnLevelLoaded;
            mLevelService.LevelCompleted     += OnLevelCompleted;
            mGameService.GameInited          += OnGameInited;
            mSettingsService.SettingsUpdated += OnSettingsUpdated;
        }

        void OnDisable() 
        {
            mLevelService.LevelLoaded        -= OnLevelLoaded;
            mLevelService.LevelCompleted     -= OnLevelCompleted;
            mGameService.GameInited          -= OnGameInited;
            mSettingsService.SettingsUpdated -= OnSettingsUpdated;
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

        private void OnSettingsUpdated() 
        {
            UpdateSfxSetting();
        }

        private void UpdateSfxSetting() 
        {
            var audioSetting = mSettingsService.GetAudioSettings();
            ReplayButtonImg.sprite = audioSetting.SfxIsOn ? SfxIsOnSprite : SfxIsOffSprite;
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
            var audioSetting = mSettingsService.GetAudioSettings();
            audioSetting.SfxIsOn = !audioSetting.SfxIsOn; // Toggle setting

            mSettingsService.SetAudioSettings(audioSetting);
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

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    /// <summary>
    /// Main entry point of the game. 
    /// It executes before than other scripts,
    /// 
    /// *** THIS SCRIPS IS ADDED TO SCRIPT EXECUTION ORDER ***
    /// </summary>
    //[DefaultExecutionOrder(-1)] // This also works but other method is more explicit
    public class Main : MonoBehaviour
    {
        [Header("Game Data")]
        public ConfigData ConfigDat;
        public PoolData PoolDat;
        public LevelData LevelDat;

        [Header("Scene Services")]
        [SerializeField] private InputService InputService;
        [SerializeField] private AudioService AudioService;

        private void Awake()
        {
            AwakeInit();
        }

        private void Start()
        {
            StartInit();
        }

        private void OnDisable()
        {
            ReleaseServices();
        }

        private void AwakeInit() 
        {
            InitConfig();
            AddServices();
            LoadGame();
        }

        private void StartInit() 
        {
            ServiceManager.Instance.Init();
        }

        private void ReleaseServices() 
        {
            ServiceManager.Instance.Release();
        }

        private void InitConfig() 
        {
            Application.targetFrameRate = ConfigDat.AppSettings.TargetFrameRate;
            DOTween.Init().SetCapacity(ConfigDat.TweenSettings.TweenCapacity, ConfigDat.TweenSettings.SequenceCapacity);
        }

        /// <summary>
        /// Add all necessary services here
        /// </summary>
        private void AddServices() 
        {
            ServiceManager.Instance.Add<ISaveService>(new SaveService());
            ServiceManager.Instance.Add<ISettingsService>(new SettingsService());
            ServiceManager.Instance.Add<ILevelService>(new LevelService(LevelDat));
            ServiceManager.Instance.Add<IGameService>(new GameService());
            ServiceManager.Instance.Add<IPoolService>(new PoolService(PoolDat));
            ServiceManager.Instance.Add<IInputService>(InputService);
            ServiceManager.Instance.Add<IAudioService>(AudioService);

            Logman.Log("All services are added!");
        }

        private void LoadGame() 
        {
            ServiceManager.Instance.Get<ISaveService>().Load();
        }
    }
}

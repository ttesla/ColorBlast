using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// Main entry point of the game. 
    /// It executes before than other scripts,
    /// </summary>
    [DefaultExecutionOrder(-1)]
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

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q)) 
            {
                ServiceManager.Instance.Get<IGameService>().StartSession(5, 5);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                ServiceManager.Instance.Get<IGameService>().StartSession(6, 6);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ServiceManager.Instance.Get<IGameService>().StartSession(7, 7);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ServiceManager.Instance.Get<IGameService>().StartSession(8, 8);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ServiceManager.Instance.Get<IGameService>().StartSession(9, 9);
            }
        }
#endif

        private void AwakeInit() 
        {
            InitConfig();
            AddServices();
        }

        private void StartInit() 
        {
            ServiceManager.Instance.Init();
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
        }
    }
}

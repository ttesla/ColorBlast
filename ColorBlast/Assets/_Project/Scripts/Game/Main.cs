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
        [Header("Configuration Data")]
        public ConfigData Config;
        public PoolData PoolDat;

        [Header("Scene Services")]
        [SerializeField] private InputService InputService;

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
                ServiceManager.Instance.Get<IGameService>().StartSession(6, 7);
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
            Application.targetFrameRate = Config.AppSettings.TargetFrameRate;
            DOTween.Init().SetCapacity(Config.TweenSettings.TweenCapacity, Config.TweenSettings.SequenceCapacity);
        }

        /// <summary>
        /// Add all necessary services here
        /// </summary>
        private void AddServices() 
        {
            ServiceManager.Instance.Add<IAudioService>(new AudioService());
            ServiceManager.Instance.Add<ISaveService>(new SaveService());
            ServiceManager.Instance.Add<IGameService>(new GameService());
            ServiceManager.Instance.Add<ISettingsService>(new SettingsService());
            ServiceManager.Instance.Add<IInputService>(InputService);
            ServiceManager.Instance.Add<IPoolService>(new PoolService(PoolDat));
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        private void OnDisable()
        {
            ReleaseServices();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q)) 
            {
                ServiceManager.Instance.Get<ILevelService>().LoadLevel(0);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                ServiceManager.Instance.Get<ILevelService>().LoadLevel(1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ServiceManager.Instance.Get<ILevelService>().LoadLevel(2);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ServiceManager.Instance.Get<ILevelService>().LoadLevel(3);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ServiceManager.Instance.Get<ILevelService>().LoadLevel(4);
            }
            else if(Input.GetKeyDown(KeyCode.A)) 
            {
                ServiceManager.Instance.Get<IGameService>().StartSession();
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
        }
    }
}

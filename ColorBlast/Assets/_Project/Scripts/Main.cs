using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace ColorBlast
{
    public class Main : MonoBehaviour
    {
        [Header("Configuration Settings")]
        public ConfigData Config;

        [Header("Game Manager")]
        public GameManager GameMan;

        private void Awake()
        {
            Init();
        }

        private void Init() 
        {
            Application.targetFrameRate = Config.AppSettings.TargetFrameRate;
            DOTween.Init().SetCapacity(Config.TweenSettings.TweenCapacity, Config.TweenSettings.SequenceCapacity);
            ServiceManager.Instance.Init();
            GameMan.Init();
        }

        private void Start()
        {
            // Test Service Code
            ServiceManager.Instance.Get<IAudioService>().Play(null);
        }
    }
}

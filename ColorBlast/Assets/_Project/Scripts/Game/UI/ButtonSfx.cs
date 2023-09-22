using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        private Button mButton;
        private IAudioService mAudioService;

        void Awake()
        {
            mButton = GetComponent<Button>();
            mAudioService = ServiceManager.Instance.Get<IAudioService>();
            
            mButton.onClick.AddListener(() => 
            {
                mAudioService.PlaySfx(SfxType.ButtonClick);
            });
        }
    }
}

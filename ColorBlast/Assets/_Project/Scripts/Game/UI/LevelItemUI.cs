using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class LevelItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI LevelNumber;
        [SerializeField] private TextMeshProUGUI LevelName;
        [SerializeField] private Button OpenLevelButton;

        private int mLevelIndex = 0;
        private ILevelService mLevelService;

        private void Awake()
        {
            OpenLevelButton.onClick.AddListener(OnOpenLevelClicked);
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
        }

        public void Fill(int levelNumber, Level level) 
        {
            LevelName.SetText(level.Name);
            LevelNumber.SetText("Level: " + levelNumber.ToString());

            mLevelIndex = levelNumber - 1;
        }

        private void OnOpenLevelClicked() 
        {
            mLevelService.LoadLevel(mLevelIndex);
        }
    }
}

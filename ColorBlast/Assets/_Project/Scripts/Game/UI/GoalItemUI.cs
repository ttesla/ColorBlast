using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class GoalItemUI : MonoBehaviour
    {
        [SerializeField] private Image BackgroundImage;
        [SerializeField] private TextMeshProUGUI CountText;
        [SerializeField] private TileData TileDat;

        public void SetGoal(LevelGoal levelGoal) 
        {
            SetBackgroundColor(levelGoal.TargetTile);
            UpdateCount(levelGoal.Count);
        }

        public void UpdateCount(int count) 
        {
            CountText.SetText("x" + count);
        }

        private void SetBackgroundColor(TileType tileType) 
        {
            BackgroundImage.color = TileDat.GetTileColor(tileType);
        }
    }
}

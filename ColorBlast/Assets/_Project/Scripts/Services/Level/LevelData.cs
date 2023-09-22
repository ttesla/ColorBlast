using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    [Serializable]
    public class Level
    {
        public string Name;

        [Range(GameConstants.MinBoardSize, GameConstants.MaxBoardSize)]
        public int Width;

        [Range(GameConstants.MinBoardSize, GameConstants.MaxBoardSize)]
        public int Height;

        public List<LevelGoal> Goals;
    }

    [Serializable]
    public class LevelGoal 
    {
        public TileType TargetTile;
        public int Count;
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<Level> LevelList;
    }
}

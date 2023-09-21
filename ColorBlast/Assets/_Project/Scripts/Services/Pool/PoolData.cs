using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public enum PoolType
    {
        TileRed,
        TileGreen,
        TileBlue,
        TileYellow
    }

    [Serializable]
    public class PoolSetting
    {
        public PoolType PoolType;
        public GameObject PoolPrefab;
        public int Size;
    }

    [CreateAssetMenu(fileName = "PoolData", menuName = "ScriptableObjects/PoolData", order = 1)]
    public class PoolData : ScriptableObject
    {
        public PoolSetting[] PoolSettings;
    }
}

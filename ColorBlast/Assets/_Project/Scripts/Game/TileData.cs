using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    [Serializable]
    public class TileInfo 
    {
        public TileType TType;
        public Color TColor;
    }

    /// <summary>
    /// Color, sprite info of the Tiles are stored here
    /// </summary>
    [CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
    public class TileData : ScriptableObject
    {
        public List<TileInfo> Tiles;

        public Color GetTileColor(TileType tileType) 
        {
            var tile = Tiles.Find(x => x.TType == tileType);

            if(tile != null) 
            {
                return tile.TColor;
            }
            else 
            {
                return Color.magenta;
            }
        }
    }
}

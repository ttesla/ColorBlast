using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// Slots are single Cell in the Board which holds the Tiles inside them
    /// </summary>
    public class Slot
    {
        public Tile TheTile { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsEmpty => TheTile == null;

        public Slot(Tile tile, int x, int y)
        {
            X = x;
            Y = y;
            Position = tile.transform.localPosition;

            SetTile(tile);
        }

        public void SetTile(Tile tile)
        {
            TheTile = tile;
            TheTile.SetCoord(X, Y);
        }

        public void Clear()
        {
            TheTile = null;
        }
    }
}

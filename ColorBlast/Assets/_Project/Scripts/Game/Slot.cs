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
        public Tile TheTile     { get; private set; }
        public int X            { get; private set; } // X index
        public int Y            { get; private set; } // Y index
        public Vector3 Position { get; private set; }
        public bool IsEmpty => TheTile == null;

        public Slot(Tile tile, int x, int y, Vector3 position)
        {
            X = x;
            Y = y;
            Position = position;

            SetTile(tile, true);
        }

        /// <summary>
        /// Puts the tile into the slot. 
        /// Sets the position if desired.
        /// </summary>
        public void SetTile(Tile tile, bool setPosition)
        {
            TheTile = tile;
            TheTile.SetCoord(X, Y);

            if(setPosition)
            {
                TheTile.SetPosition(Position);
            }
        }

        public void Clear()
        {
            TheTile = null;
        }
    }
}

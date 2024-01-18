using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class BombTile : Tile
    {
        public override bool CheckTilesToPop(Slot[,] boardMap, Tile tile, out List<Tile> popTileList)
        {
            popTileList = new List<Tile>();

            // SELF
            popTileList.Add(tile);

            // LEFT
            int nextX = tile.X - 1;
            int nextY = tile.Y;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // LEFT UP
            nextX = tile.X - 1;
            nextY = tile.Y - 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // RIGHT
            nextX = tile.X + 1;
            nextY = tile.Y;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // RIGHT UP
            nextX = tile.X + 1;
            nextY = tile.Y - 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // UP
            nextX = tile.X;
            nextY = tile.Y - 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // DOWN
            nextX = tile.X;
            nextY = tile.Y + 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // LEFT DOWN
            nextX = tile.X - 1;
            nextY = tile.Y + 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // RIGHT DOWN
            nextX = tile.X + 1;
            nextY = tile.Y + 1;
            AddToBombExplosion(boardMap, nextX, nextY, popTileList);

            // SFX
            ServiceManager.Instance.Get<IAudioService>().PlaySfx(SfxType.BoosterBomb);

            return true;
        }

        private void AddToBombExplosion(Slot[,] boardMap, int x, int y, List<Tile> popTileList)
        {
            int height = boardMap.GetLength(0);
            int width = boardMap.GetLength(1);

            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                var slot = boardMap[y, x];

                if (!slot.IsEmpty)
                {
                    popTileList.Add(slot.TheTile);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class ColorTile : Tile
    {
        /// <summary>
        /// Try to find connected tiles to Pop with BFS algorithm
        /// </summary>
        public override bool CheckTilesToPop(Slot[,] boardMap, Tile tile, out List<Tile> popTileList)
        {
            bool result          = false;
            popTileList          = new List<Tile>();
            var targetTileType   = tile.TType;
            HashSet<int> visited = new HashSet<int>();
            Queue<Tile> queue    = new Queue<Tile>();
            int width            = boardMap.GetLength(1);

            queue.Enqueue(tile);
            visited.Add(BoardHelper.CoordToIndex(tile.X, tile.Y, width));

            while (queue.Count > 0)
            {
                // Deque and add to pop tile list
                var currentTile = queue.Dequeue();
                popTileList.Add(currentTile);

                // Visit neighbors //

                // LEFT
                int nextX = currentTile.X - 1;
                int nextY = currentTile.Y;
                CheckNextTileIsValid(boardMap, nextX, nextY, targetTileType, visited, queue);

                // RIGHT
                nextX = currentTile.X + 1;
                nextY = currentTile.Y;
                CheckNextTileIsValid(boardMap, nextX, nextY, targetTileType, visited, queue);

                // UP
                nextX = currentTile.X;
                nextY = currentTile.Y - 1;
                CheckNextTileIsValid(boardMap, nextX, nextY, targetTileType, visited, queue);

                // DOWN
                nextX = currentTile.X;
                nextY = currentTile.Y + 1;
                CheckNextTileIsValid(boardMap, nextX, nextY, targetTileType, visited, queue);

            }

            // If we have more than 1 to pop, then its true
            result = popTileList.Count > 1;

            return result;
        }

        private void CheckNextTileIsValid(Slot[,] boardMap, int x, int y, TileType targetTileType, HashSet<int> visited, Queue<Tile> queue)
        {
            int height = boardMap.GetLength(0);
            int width  = boardMap.GetLength(1);

            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                int index = BoardHelper.CoordToIndex(x, y, width);

                if (!visited.Contains(index))
                {
                    var slot = boardMap[y, x];

                    if (!slot.IsEmpty && slot.TheTile.TType == targetTileType)
                    {
                        queue.Enqueue(slot.TheTile);
                        visited.Add(index);
                    }
                }
            }
        }
    }
}

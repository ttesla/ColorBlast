using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ColorBlast
{
    /// <summary>
    /// Helper class for Board mechanics and calculations
    /// </summary>
    public class BoardHelper 
    {
        private IPoolService mPoolService;
        private Slot[,] mBoardMap;

        private int mWidth;
        private int mHeight;

        public BoardHelper(Slot[,] boardMap, IPoolService poolService) 
        {
            mPoolService = poolService;
            mBoardMap    = boardMap;

            mHeight = mBoardMap.GetLength(0);
            mWidth  = mBoardMap.GetLength(1);
        }

        /// <summary>
        /// This is quite ugly but managable
        /// </summary>
        public Tile GetRandomBasicTile()
        {
            int random = UnityEngine.Random.Range(0, Tile.BasicTileCount);
            Tile tile = null;

            PoolType poolType = PoolType.TileRed;

            switch (random)
            {
                case 0:
                    poolType = PoolType.TileRed;
                    break;

                case 1:
                    poolType = PoolType.TileGreen;
                    break;

                case 2:
                    poolType = PoolType.TileBlue;
                    break;

                case 3:
                    poolType = PoolType.TileYellow;
                    break;
            }

            tile = mPoolService.Get<Tile>(poolType);
            tile.Init(poolType);

            return tile;
        }

        /// <summary>
        /// Try to find connected tiles to Pop with BFS algorithm
        /// </summary>
        public bool FindConnectedTiles(Tile tile, out List<Tile> popTileList)
        {
            bool result          = false;
            popTileList          = new List<Tile>();
            var targetTileType   = tile.TType;
            HashSet<int> visited = new HashSet<int>();
            Queue<Tile> queue    = new Queue<Tile>();

            queue.Enqueue(tile);
            visited.Add(CoordToIndex(tile.X, tile.Y));

            while (queue.Count > 0)
            {
                // Deque and add to pop tile list
                var currentTile = queue.Dequeue();
                popTileList.Add(currentTile);

                // Visit neighbors //

                // LEFT
                int nextX = currentTile.X - 1;
                int nextY = currentTile.Y;
                CheckNextTileIsValid(nextX, nextY, targetTileType, visited, queue);

                // RIGHT
                nextX = currentTile.X + 1;
                nextY = currentTile.Y;
                CheckNextTileIsValid(nextX, nextY, targetTileType, visited, queue);

                // UP
                nextX = currentTile.X;
                nextY = currentTile.Y - 1;
                CheckNextTileIsValid(nextX, nextY, targetTileType, visited, queue);

                // DOWN
                nextX = currentTile.X;
                nextY = currentTile.Y + 1;
                CheckNextTileIsValid(nextX, nextY, targetTileType, visited, queue);

            }

            // If we have more than 1 to pop, then its true
            result = popTileList.Count > 1;

            return result;
        }

        private void CheckNextTileIsValid(int x, int y, TileType targetTileType, HashSet<int> visited, Queue<Tile> queue)
        {
            if (x >= 0 && x < mWidth &&
                y >= 0 && y < mHeight)
            {
                int index = CoordToIndex(x, y);

                if (!visited.Contains(index))
                {
                    var slot = mBoardMap[y, x];

                    if (!slot.IsEmpty && slot.TheTile.TType == targetTileType)
                    {
                        queue.Enqueue(mBoardMap[y, x].TheTile);
                        visited.Add(index);
                    }
                }
            }
        }

        /// <summary>
        /// This makes tiles fall down if they have an empty space (Slot) underneath them.
        /// </summary>
        public bool GravitySolver()
        {
            bool solved = true;

            // Apply gravitational wave from bottom to top for faster resolution
            for (int y = mHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    // Check if current slot is not empty and down below slot is empty
                    var currentSlot = mBoardMap[y, x];

                    if (!currentSlot.IsEmpty)
                    {
                        int nextY = y + 1;

                        if (nextY < mHeight && mBoardMap[nextY, x].IsEmpty)
                        {
                            // Then move current tile to the slot down below
                            // Tile positions are saved to be moved later
                            var belowSlot = mBoardMap[nextY, x];
                            belowSlot.SetTile(currentSlot.TheTile, false);
                            currentSlot.Clear();
                            belowSlot.TheTile.RecordMoveTo(belowSlot.Position);
                            solved = false;
                        }
                    }
                }
            }

            return solved;
        }

        /// <summary>
        /// Get list of empty slots
        /// </summary>
        public List<Slot> GetEmptySlots() 
        {
            var emptySlots = new List<Slot>();

            for(int y = 0; y < mHeight; y++) 
            {
                for (int x = 0; x < mWidth; x++) 
                {
                    var slot = mBoardMap[y, x];
                    
                    if (slot.IsEmpty) 
                    {
                        emptySlots.Add(slot);
                    }
                }
            }

            return emptySlots;
        }

        /// <summary>
        /// Checks the board if there is at least 1 Tile to pop
        /// or else, board is dead. We need to reset the board.
        /// </summary>
        public bool IsThereAnyValidMove() 
        {
            for (int y = 0; y < mHeight - 1; y++)
            {
                for (int x = 0; x < mWidth - 1; x++)
                {
                    var horizonTal_1 = mBoardMap[y, x];
                    var horizonTal_2 = mBoardMap[y, x + 1];

                    var vertical_1 = mBoardMap[y, x];
                    var vertical_2 = mBoardMap[y + 1, x];

                    if(horizonTal_1.TheTile.TType == horizonTal_2.TheTile.TType) 
                    {
                        return true;
                    }

                    if(vertical_1.TheTile.TType == vertical_2.TheTile.TType) 
                    {
                        return true;
                    }
                }
            }

            // If we are up to here.
            // I'm afraid my friend, we need a new random board!
            return false;
        }

        /// <summary>
        /// Tiles record move to position on themselves.
        /// They start to move when move command is applied.
        /// </summary>
        public void ApplyTileMove()
        {
            for (int y = 0; y < mHeight; y++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    var slot = mBoardMap[y, x];
                    
                    if (!slot.IsEmpty)
                    {
                        slot.TheTile.ApplyMove();
                    }
                }
            }
        }

        /// <summary>
        /// Drop new tiles from heaven
        /// </summary>
        public void DropNewTiles(Transform parent, float fallAboveHeight)
        {
            var fallPosY   = mBoardMap[0, 0].Position.y + fallAboveHeight;
            var emptySlots = GetEmptySlots();

            // Create a new random tile for each empty slot
            foreach (var slot in emptySlots)
            {
                var randomTile = GetRandomBasicTile();
                randomTile.transform.SetParent(parent);

                // Set tile's initial position somewhere above the board.
                // Put the tile into the slot and record move position to animate later
                var tileStartPos = slot.Position + new Vector3(0.0f, fallPosY, 0.0f);
                randomTile.SetPosition(tileStartPos);
                slot.SetTile(randomTile, false);
                randomTile.RecordMoveTo(slot.Position);
                randomTile.ApplyMove();
            }
        }

        public void ClearTheBoard() 
        {
            for (int y = 0; y < mHeight; y++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    var slot = mBoardMap[y, x];

                    if (!slot.IsEmpty) 
                    {
                        slot.TheTile.Pop();
                        slot.Clear();
                    }
                }
            }
        }

        private int CoordToIndex(int x, int y)
        {
            return y * mWidth + x;
        }
    }
}

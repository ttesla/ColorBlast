using System;
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
        public int X            { get; private set; }
        public int Y            { get; private set; }
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


    public class Board : MonoBehaviour
    {
        // TODO: Get Tiles from Pool
        // TODO: Make a proper offset stuff
        // TODO: Make it enable for 5x5, 5x8 upto 9x9 tiles
        public Tile[] Tiles;

        public Slot[,] BoardMap;

        public void Init(int width, int height) 
        {
            BoardMap = new Slot[height, width];

            for(int y = 0; y < height; y++) 
            {
                for(int x = 0; x < width; x++) 
                {
                    var randomTile = Tiles[UnityEngine.Random.Range(0, Tiles.Length)];
                    var tile = GameObject.Instantiate(randomTile, transform);
                    
                    tile.transform.localPosition = new Vector3(x - 2.5f, -y, 0.0f);
                    BoardMap[y, x] = new Slot(tile, x, y);
                }
            }
        }

        public void TryToPopTile(Tile tile) 
        {
            Debug.Log("Try to Pop Tile!");
            
            if(FindPopTiles(tile, out List<Tile> popTiles)) 
            {
                Debug.Log("Found tiles to pop!");
                PopTiles(popTiles);

                // TODO: Make a small delay here...
                DropExistingTiles();
            }
        }

        /// <summary>
        /// Try to find connected tiles to Pop with BFS algorithm
        /// </summary>
        private bool FindPopTiles(Tile tile, out List<Tile> popTileList) 
        {
            bool result = false;
            popTileList = new List<Tile>();

            var targetTileType = tile.TileType;

            HashSet<int> visited = new HashSet<int>();
            Queue<Tile> queue    = new Queue<Tile>();

            queue.Enqueue(tile);
            visited.Add(CoordToIndex(tile.X, tile.Y));

            while(queue.Count > 0) 
            {
                // Deque and add to pop tile list
                var currentTile = queue.Dequeue();
                popTileList.Add(currentTile);

                // Visit neighbours //

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
            int height = BoardMap.GetLength(0);
            int width  = BoardMap.GetLength(1);

            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                int index = CoordToIndex(x, y);

                if (!visited.Contains(index))
                {
                    var slot = BoardMap[y, x];

                    if (!slot.IsEmpty && slot.TheTile.TileType == targetTileType)
                    {
                        queue.Enqueue(BoardMap[y, x].TheTile);
                        visited.Add(index);
                    }
                }
            }
        }

        private void PopTiles(List<Tile> tilesToPop) 
        {
            foreach (Tile tile in tilesToPop)
            {
                BoardMap[tile.Y, tile.X].Clear();
                tile.Pop();
            }
        }

        private void DropExistingTiles()
        {
            int programmerSanityCheck = 0;

            while (true) 
            {
                // If this is false, we may have holes in the Board,
                // we check again until no tile falls.
                if (GravitySolver()) 
                {
                    break;
                }

                programmerSanityCheck++;

                // This is here to check
                if(programmerSanityCheck > 100) 
                {
                    Debug.LogError("Critic Error: Gravity solver ended up in a dead loop!");
                    break;
                }
            }

            ApplyTileMove();
        }

        private bool GravitySolver() 
        {
            int height = BoardMap.GetLength(0);
            int width = BoardMap.GetLength(1);
            bool solved = true;

            // Apply gravitanional wave from bottom to top for faster resolution
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    // Check if current slot is not empty and down below slot is empty
                    var currentSlot = BoardMap[y, x];

                    if (!currentSlot.IsEmpty)
                    {
                        int nextY = y + 1;

                        if (nextY < height && BoardMap[nextY, x].IsEmpty)
                        {
                            // Then move current tile to the slot down below
                            var belowSlot = BoardMap[nextY, x];
                            belowSlot.SetTile(currentSlot.TheTile);
                            currentSlot.Clear();
                            belowSlot.TheTile.RecordMoveTo(belowSlot.Position);
                            solved = false;
                        }
                    }
                }
            }

            return solved;
        }

        private void ApplyTileMove() 
        {
            int height = BoardMap.GetLength(0);
            int width = BoardMap.GetLength(1);
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!BoardMap[y, x].IsEmpty)
                    {
                        BoardMap[y, x].TheTile.ApplyMove();
                    }
                }
            }
        }

        private void DropNewTiles() 
        {

        }

        private int CoordToIndex(int x, int y) 
        {
            int width = BoardMap.GetLength(1);

            return y * width + x;
        }
    }
}

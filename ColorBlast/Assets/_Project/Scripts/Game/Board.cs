using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// Slots are single Cell in the Board which holds the Tiles inside them
    /// </summary>
    public class Slot 
    {
        public Tile TheTile { get; private set; }

        public bool IsEmpty => TheTile == null;

        public Slot(Tile tile, int x, int y) 
        {
            SetTile(tile, x, y);
        }

        public void SetTile(Tile tile, int x, int y) 
        {
            TheTile = tile;
            TheTile.SetCoord(x, y);
        }

        public void Clear() 
        {
            TheTile = null;
        }
    }


    public class Board : MonoBehaviour
    {
        // TODO: Get Tiles from Pool
        public Tile[] Tiles;

        public Slot[,] BoardMap;

        public void Init(int width, int height) 
        {
            BoardMap = new Slot[height, width];

            for(int y = 0; y < height; y++) 
            {
                for(int x = 0; x < width; x++) 
                {
                    var randomTile = Tiles[Random.Range(0, Tiles.Length)];
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
            }
        }

        /// <summary>
        /// Try to find connected tiles to Pop with BFS search algorithm
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

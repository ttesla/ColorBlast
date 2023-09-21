using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class Board : MonoBehaviour
    {
        public float DelayAfterPop;

        public Slot[,] BoardMap;

        //Service references
        private IGameService mGameService;
        private IAudioService mAudioService;
        private IInputService mInputService;
        private IPoolService mPoolService;

        private void Start()
        {
            mGameService  = ServiceManager.Instance.Get<IGameService>();
            mAudioService = ServiceManager.Instance.Get<IAudioService>();
            mInputService = ServiceManager.Instance.Get<IInputService>();
            mPoolService  = ServiceManager.Instance.Get<IPoolService>(); 

            mGameService.SessionStarted += OnGameSessionStarted;
            mInputService.Tapped        += OnTapped;
        }

        private void OnDisable()
        {
            mGameService.SessionStarted -= OnGameSessionStarted;
            mInputService.Tapped        -= OnTapped;
        }

        private void OnTapped(Tile tile)
        {
            TryToPopTile(tile);
        }

        private void OnGameSessionStarted(SessionParameters sessionParams)
        {
            Init(sessionParams.Width, sessionParams.Height);
        }

        private void Init(int width, int height) 
        {
            BoardMap = new Slot[height, width];

            for(int y = 0; y < height; y++) 
            {
                for(int x = 0; x < width; x++) 
                {
                    var randomTile = GetRandomBasicTile();
                    randomTile.transform.SetParent(transform);

                    randomTile.transform.localPosition = new Vector3(x - 2.5f, -y, 0.0f);
                    BoardMap[y, x] = new Slot(randomTile, x, y);
                }
            }
        }

        /// <summary>
        /// This is quite ugly but managable
        /// </summary>
        private Tile GetRandomBasicTile() 
        {
            int random = UnityEngine.Random.Range(0, Tile.BasicTileCount);
            Tile tile = null;

            PoolType poolType = PoolType.TileRed;

            switch(random) 
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

        private void TryToPopTile(Tile tile) 
        {
            if(FindConnectedTiles(tile, out List<Tile> popTiles)) 
            {
                PopTiles(popTiles);

                // Give some time for popping animations then make tiles fall... 
                DOVirtual.DelayedCall(DelayAfterPop, () => 
                {
                    DropExistingTiles();
                }, false);
            }
        }

        private void PopTiles(List<Tile> tilesToPop)
        {
            foreach (Tile tile in tilesToPop)
            {
                BoardMap[tile.Y, tile.X].Clear();
                tile.Pop();
            }

            // TODO: Play pop sound here
            //mAudioService.Play()
        }

        /// <summary>
        /// Try to find connected tiles to Pop with BFS algorithm
        /// </summary>
        private bool FindConnectedTiles(Tile tile, out List<Tile> popTileList) 
        {
            bool result = false;
            popTileList = new List<Tile>();

            var targetTileType = tile.TType;

            HashSet<int> visited = new HashSet<int>();
            Queue<Tile> queue    = new Queue<Tile>();

            queue.Enqueue(tile);
            visited.Add(CoordToIndex(tile.X, tile.Y));

            while(queue.Count > 0) 
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
            int height = BoardMap.GetLength(0);
            int width  = BoardMap.GetLength(1);

            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                int index = CoordToIndex(x, y);

                if (!visited.Contains(index))
                {
                    var slot = BoardMap[y, x];

                    if (!slot.IsEmpty && slot.TheTile.TType == targetTileType)
                    {
                        queue.Enqueue(BoardMap[y, x].TheTile);
                        visited.Add(index);
                    }
                }
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

                // This is here to check for infinite loop. Probably won't ever happen but you never know!
                if(programmerSanityCheck > 100) 
                {
                    Debug.LogError("Critic Error: Gravity solver ended up in a dead loop!");
                    break;
                }
            }

            ApplyTileMove();
        }

        /// <summary>
        /// This makes tiles fall down if they have an empty space (Slot) underneath them.
        /// </summary>
        private bool GravitySolver() 
        {
            int height = BoardMap.GetLength(0);
            int width = BoardMap.GetLength(1);
            bool solved = true;

            // Apply gravitational wave from bottom to top for faster resolution
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

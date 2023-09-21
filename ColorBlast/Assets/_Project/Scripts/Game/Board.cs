using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class Board : MonoBehaviour
    {
        public float DelayAfterPop;
        public float FallAboveHeight;

        //Board Matrix Map
        private Slot[,] mBoardMap;

        //Service references
        private IGameService mGameService;
        private IAudioService mAudioService;
        private IInputService mInputService;
        private IPoolService mPoolService;

        private BoardHelper mBoardHelper;
        private bool mIgnoreInput;

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
            // Ignore input if previous move is not finished yet
            if (mIgnoreInput)
            {
                return;
            }

            TryToPopTile(tile);
        }

        private void OnGameSessionStarted(SessionParameters sessionParams)
        {
            Init(sessionParams.Width, sessionParams.Height);
        }

        private void Init(int width, int height) 
        {
            mBoardMap = new Slot[height, width];
            mBoardHelper = new BoardHelper(mBoardMap, mPoolService);

            for (int y = 0; y < height; y++) 
            {
                for(int x = 0; x < width; x++) 
                {
                    var randomTile = mBoardHelper.GetRandomBasicTile();
                    randomTile.transform.SetParent(transform);

                    var position = new Vector3(x - 2.5f, -y, 0.0f);
                    mBoardMap[y, x] = new Slot(randomTile, x, y, position);
                }
            }
        }

        private void TryToPopTile(Tile tile) 
        {
            if(mBoardHelper.FindConnectedTiles(tile, out List<Tile> popTiles)) 
            {
                mIgnoreInput = true;
                PopTiles(popTiles);

                // Give some time for popping animations then make new tiles fall... 
                DOVirtual.DelayedCall(DelayAfterPop, () => 
                {
                    AfterPop();
                }, 
                false);
            }
        }

        private void PopTiles(List<Tile> tilesToPop)
        {
            foreach (Tile tile in tilesToPop)
            {
                mBoardMap[tile.Y, tile.X].Clear();
                tile.Pop();
            }

            // TODO: Play pop sound here
            //mAudioService.Play()
        }

        private void AfterPop() 
        {
            DropExistingTiles();
            mBoardHelper.DropNewTiles(transform, FallAboveHeight);

            EnsureBoardHasValidMove(() => 
            {
                // Now you can continue playing...
                mIgnoreInput = false;
            });
        }

        private void EnsureBoardHasValidMove(Action result) 
        {
            if (mBoardHelper.IsThereAnyValidMove())
            {
                // Good, invoke the callback right away.
                result?.Invoke();
            }
            else
            {
                // Not good, we try to get a valid board by trying some random trials...
                Logman.LogWarning("No valid move, making a new random board!...");
                StartCoroutine(MakeNewRandomBoardRoutine(result));
            }
        }

        private IEnumerator MakeNewRandomBoardRoutine(Action result) 
        {
            // First clear the board, then drop new tiles.
            // But we keep doing this, until we are sure the board is valid
            // This may deadlock the game. So we won't try it forever. 

            int loopCounter = 0;

            while (true) 
            {
                mBoardHelper.ClearTheBoard();
                yield return new WaitForSeconds(DelayAfterPop * 2.0f);
                mBoardHelper.DropNewTiles(transform, FallAboveHeight);

                if (mBoardHelper.IsThereAnyValidMove()) 
                {
                    // Nice, we have a valid board now
                    result?.Invoke();
                    yield break;
                }

                // This is here to check for infinite loop.
                if (loopCounter++ > 100)
                {
                    Debug.LogError("Critical Error: MakeNewRandomBoardRoutine failed to make a valid random board!");
                    break;
                }
            }
            
            // If we are here, we can continue with broken board.
            // At least player can quit the app :)
            result?.Invoke();
        }

        private void DropExistingTiles()
        {
            int loopCounter = 0;

            while (true) 
            {
                // If this is false, we may have holes in the Board,
                // we check again until no tile falls.
                if (mBoardHelper.GravitySolver()) 
                {
                    break;
                }

                // This is here to check for infinite loop.
                // Probably won't ever happen but you never know!
                if(loopCounter++ > 100) 
                {
                    Debug.LogError("Critical Error: Gravity solver ended up in a dead loop!");
                    break;
                }
            }

            mBoardHelper.ApplyTileMove();
        }
    }
}

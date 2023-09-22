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
        [Header("Board Settings")]
        [SerializeField] private float DelayAfterPop;
        [SerializeField] private float FallAboveHeight;

        [Header("Camera View Size")]
        [SerializeField] private float MinCameraSize;
        [SerializeField] private float MaxCameraSize;

        //Board Matrix Map
        private Slot[,] mBoardMap;

        //Service references
        private IGameService mGameService;
        private IAudioService mAudioService;
        private IInputService mInputService;
        private IPoolService mPoolService;
        private ILevelService mLevelService;

        private BoardHelper mBoardHelper;
        private bool mAllowInput;

        private void Awake()
        {
            mGameService  = ServiceManager.Instance.Get<IGameService>();
            mAudioService = ServiceManager.Instance.Get<IAudioService>();
            mInputService = ServiceManager.Instance.Get<IInputService>();
            mPoolService  = ServiceManager.Instance.Get<IPoolService>();
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
        }

        private void OnEnable()
        {
            mLevelService.LevelLoaded   += OnLevelLoaded;
            mGameService.SessionStarted += OnGameSessionStarted;
            mGameService.SessionEnded   += OnGameSessionEnded;
            mInputService.Tapped        += OnTapped;
        }

        private void OnDisable()
        {
            mLevelService.LevelLoaded   -= OnLevelLoaded;
            mGameService.SessionStarted -= OnGameSessionStarted;
            mGameService.SessionEnded   -= OnGameSessionEnded;
            mInputService.Tapped        -= OnTapped;
        }

        #region ServiceEvents

        private void OnLevelLoaded(Level level)
        {
            CreateBoard(level.Width, level.Height);
        }

        private void OnTapped(Tile tile)
        {
            if (mAllowInput)
            {
                TryToPopTile(tile);
            }
        }

        private void OnGameSessionStarted()
        {
            // Check if the board is valid
            EnsureBoardHasValidMove(() =>
            {
                // We can play now.
                mAllowInput = true;
            });
        }

        private void OnGameSessionEnded()
        {
            if(mBoardHelper != null) 
            {
                mBoardHelper.ClearTheBoard();
            }
            
            mAllowInput = false;
        }

        #endregion

        private void CreateBoard(int width, int height) 
        {
            // If we already created before, clear the old board.
            if(mBoardHelper != null) 
            {
                mBoardHelper.ClearTheBoard();
            }

            mBoardMap    = new Slot[height, width];
            mBoardHelper = new BoardHelper(mBoardMap, mPoolService);
            mAllowInput  = false;

            float xOffset = width  / 2.0f - 0.5f;
            float yOffset = height / 2.0f;

            // Setup the board for the first time
            for (int y = 0; y < height; y++) 
            {
                for(int x = 0; x < width; x++) 
                {
                    var randomTile = mBoardHelper.GetRandomBasicTile();
                    randomTile.transform.SetParent(transform);

                    var position = new Vector3(x - xOffset, -(y - yOffset), 0.0f);
                    mBoardMap[y, x] = new Slot(randomTile, x, y, position);
                }
            }

            AdjustCameraViewSize(width);
        }

        private void AdjustCameraViewSize(int width) 
        {
            // These calculations are tailored for Portrait orientation.
            float minRatio     = (float)GameConstants.MinBoardSize / GameConstants.MaxBoardSize;
            float ratio        = (float)width / GameConstants.MaxBoardSize;
            float screenRatio  = ((float)Screen.width / Screen.height) * 2.11f;
            float screenFactor = 1.0f / screenRatio;

            var result = MathUtil.Remap(minRatio, 1.0f, MinCameraSize, MaxCameraSize, ratio);
            Camera.main.orthographicSize = result * screenFactor;
        }

        private void TryToPopTile(Tile tile) 
        {
            if(mBoardHelper.FindConnectedTiles(tile, out List<Tile> popTiles)) 
            {
                mAllowInput = false;
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

                // Notify level service when a tile Pops
                mLevelService.NotifyTilePop(tile.TType, 1);
            }

            mAudioService.PlaySfx(SfxType.Pop);
        }

        private void AfterPop() 
        {
            DropExistingTiles();
            mBoardHelper.DropNewTiles(transform, FallAboveHeight);

            EnsureBoardHasValidMove(() => 
            {
                // Now you can continue playing...
                mAllowInput = true;
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
                mAudioService.PlaySfx(SfxType.BoardShuffle);

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

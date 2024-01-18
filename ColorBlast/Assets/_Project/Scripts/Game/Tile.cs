using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public enum TileType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Bomb
    }

    public abstract class Tile : MonoBehaviour
    {
        [SerializeField] private TileType TileType;
        [SerializeField] private TileData TileDat;

        public const int BasicTileCount = 4;

        public TileType TType => TileType;
        public int X            { get; private set; }
        public int Y            { get; private set; }
        public bool SpecialTile { get; private set; }

        private Vector3 mTargetMovePos;
        private bool mMoveDirtyFlag;

        private const float MoveDuration = 0.3f;
        private const float PopDuration  = 0.2f;
        private PoolType mPoolType;
        private bool mInited = false;


        public void Init(PoolType poolType) 
        {
            if (mInited) 
            {
                return;
            }

            var material   = GetComponent<Renderer>().material;
            material.color = TileDat.GetTileColor(TType);
            SpecialTile    = TileDat.IsSpecialTile(TType);

            mPoolType = poolType;
            mInited = true;
        }
        
        public void SetCoord(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public void SetPosition(Vector3 position) 
        {
            transform.localPosition = position;
        }

        public void RecordMoveTo(Vector3 position) 
        {
            mTargetMovePos = position;
            mMoveDirtyFlag = true;
        }

        public void ApplyMove() 
        {
            if(mMoveDirtyFlag) 
            {
                transform.DOKill();
                transform.DOLocalMove(mTargetMovePos, MoveDuration).SetEase(Ease.OutBounce);
                mMoveDirtyFlag = false;
            }
        }

        public void Pop() 
        {
            transform.DOScale(Vector3.zero, PopDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    // Return to pool
                    Recycle();
                });
        }

        public void Recycle() 
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            ServiceManager.Instance.Get<IPoolService>().Return(mPoolType, gameObject);
        }

        /// <summary>
        /// Returns true if there are any tiles to pop
        /// Tiles to be popped are filled in popTileList 
        /// </summary>
        public abstract bool CheckTilesToPop(Slot[,] boardMap, Tile tile, out List<Tile> popTileList);
    }
}

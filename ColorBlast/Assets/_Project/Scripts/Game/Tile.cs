using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace ColorBlast
{
    public enum TileType 
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType TileType;

        public const int BasicTileCount = 4;

        public TileType TType => TileType;
        public int X { get; private set; }
        public int Y { get; private set; }

        private Vector3 mTargetMovePos;
        private bool mHasMovePos;

        private const float MoveDuration = 0.3f;
        private const float PopDuration  = 0.2f;
        private PoolType mPoolType;

        public void Init(PoolType poolType) 
        {
            mPoolType = poolType;
        }
        
        public void SetCoord(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public void RecordMoveTo(Vector3 pos) 
        {
            mTargetMovePos = pos;
            mHasMovePos = true;
        }

        public void ApplyMove() 
        {
            if(mHasMovePos) 
            {
                transform.DOKill();
                transform.DOLocalMove(mTargetMovePos, MoveDuration).SetEase(Ease.OutBounce);
                mHasMovePos = false;
            }
        }

        public void Pop() 
        {
            transform.DOScale(Vector3.zero, PopDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    // Return to pool
                    ServiceManager.Instance.Get<IPoolService>().Return(mPoolType, gameObject);
                });
        }
    }
}

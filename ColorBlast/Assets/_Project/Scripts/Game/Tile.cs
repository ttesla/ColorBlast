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
        public TileType TileType;

        public int X { get; private set; }
        public int Y { get; private set; }

        private Vector3 mTargetMovePos;
        private bool mHasMovePos;
        
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
                transform.DOLocalMove(mTargetMovePos, 0.3f).SetEase(Ease.OutBounce);
            }
        }

        public void Pop() 
        {
            gameObject.SetActive(false);
            Debug.Log("Tile exploded");
        }
    }
}

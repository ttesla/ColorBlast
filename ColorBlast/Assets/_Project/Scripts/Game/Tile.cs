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
        Yellow
    }

    public class Tile : MonoBehaviour
    {
        public TileType TileType;

        public int X { get; private set; }
        public int Y { get; private set; }
        
        
        public void SetCoord(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public void Move(Vector3 pos) 
        {
            transform.DOKill();
            transform.DOLocalMove(pos, 0.1f).SetEase(Ease.OutElastic);
        }

        public void Pop() 
        {
            gameObject.SetActive(false);
            Debug.Log("Tile exploded");
        }
    }
}

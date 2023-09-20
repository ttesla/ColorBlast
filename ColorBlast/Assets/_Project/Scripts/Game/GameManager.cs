using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    // Make this as service and make proper init, game start, game end events
    public class GameManager : MonoBehaviour
    {
        public Board GameBoard;

        public event System.Action T;

        public void Init()
        {
            GameBoard.Init(8, 8);
        }
    }
}

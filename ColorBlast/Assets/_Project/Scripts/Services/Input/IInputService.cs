using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface IInputService : IService
    {
        event Action<Tile> Tapped;
    }
}

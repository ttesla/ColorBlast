using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface IPoolService : IService
    {
        T Get<T>(PoolType pType);
        void Return(PoolType pType, GameObject gameObj);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class PoolService : IPoolService
    {
        private PoolData mPoolData;
        private Dictionary<PoolType, StackPool> mPoolDict;

        public PoolService(PoolData poolData) 
        {
            mPoolData = poolData;
        }

        public void Init()
        {
            CreatePools();
            Logman.Log("PoolService - Init");
        }

        public void Release()
        {
            Logman.Log("PoolService - Release");
        }

        /// <summary>
        /// Get pooled object
        /// </summary>
        public T Get<T>(PoolType pType)
        {
            return mPoolDict[pType].Get().GetComponent<T>();
        }

        /// <summary>
        /// Return object to Pool
        /// </summary>
        public void Return(PoolType pType, GameObject gameObj) 
        {
            mPoolDict[pType].Return(gameObj);
        }

        private void CreatePools() 
        {
            mPoolDict = new Dictionary<PoolType, StackPool>();

            foreach (var poolSetting in mPoolData.PoolSettings)
            {
                var pool = new StackPool();
                pool.Create(poolSetting.PoolPrefab, poolSetting.Size);
                mPoolDict.Add(poolSetting.PoolType, pool);
            }
        }
    }
}

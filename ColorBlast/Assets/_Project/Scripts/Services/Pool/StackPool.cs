using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class StackPool
    {
        private Stack<GameObject> mPoolStack;
        private int mIndex;
        private Transform mPoolObjectTransform;
        private GameObject mPrefab;

        /// <summary>
        /// Create pool objects and store them in stack pool
        /// </summary>
        public void Create(GameObject prefab, int poolSize) 
        {
            var poolObject       = new GameObject("[POOL] - " + prefab.name);
            mPoolObjectTransform = poolObject.transform;
            mPoolStack           = new Stack<GameObject>(poolSize);
            mPrefab              = prefab;

            for(int i = 0; i < poolSize; i++) 
            {
                var newObj = CreateNew();
                mPoolStack.Push(newObj);
            }
        }

        /// <summary>
        /// Get pooled object
        /// </summary>
        public GameObject Get() 
        {
            GameObject gameObj;

            // Return from stack, if empty create new one
            if(mPoolStack.TryPop(out gameObj)) 
            {
               // Empty. If true, we should have a valid component.
            }
            else 
            {
                gameObj = CreateNew();
            }

            // Make object active and unparent.
            gameObj.SetActive(true);
            gameObj.transform.parent = null;

            return gameObj;
        }

        /// <summary>
        /// Return object to Pool
        /// </summary>
        public void Return(GameObject gameObj) 
        {
            gameObj.SetActive(false);
            gameObj.transform.parent = mPoolObjectTransform;

            mPoolStack.Push(gameObj);
        }

        private GameObject CreateNew() 
        {
            var gameObj = GameObject.Instantiate(mPrefab, mPoolObjectTransform);
            gameObj.SetActive(false);

            return gameObj;
        }
    }
}

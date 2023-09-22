using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// Dummy Save Service, added for testing the ServiceManager. 
    /// Left in the project as an example for making a different Save Service implementation. 
    /// </summary>
    public class DummySaveService : ISaveService
    {
        public event Action SaveLoaded;

        public void Init()
        {
            Debug.Log("DummySaveService - Init");
        }

        public void Release()
        {
            Debug.Log("DummySaveService - Release");
        }

        public void Save() 
        {
            Debug.Log("DummySaveService - Save");
        }

        public void DeleteSave()
        {
            Debug.Log("DummySaveService - DeleteSave");
        }

        public bool IsReady()
        {
            Debug.Log("DummySaveService - IsReady");
            return true;
        }

        public SaveData GetSaveData()
        {
            Debug.Log("DummySaveService - GetSaveData");
            return null;
        }

        public SaveData Load()
        {
            SaveLoaded?.Invoke();
            Debug.Log("DummySaveService - Load");
            return null;
        }
    }
}

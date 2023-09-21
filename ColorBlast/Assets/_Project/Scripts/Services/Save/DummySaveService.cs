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
    }
}

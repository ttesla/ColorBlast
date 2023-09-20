using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
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

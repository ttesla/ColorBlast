using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class SaveService : ISaveService
    {
        public void Init()
        {
            Debug.Log("SaveService - Init");
        }

        public void Release()
        {
            Debug.Log("SaveService - Release");
        }

        public void Save()
        {
            Debug.Log("SaveService - Save");
        }

        public void DeleteSave()
        {
            Debug.Log("SaveService - DeleteSave");
        }
    }
}

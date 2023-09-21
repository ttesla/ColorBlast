using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class SaveService : ISaveService
    {
        public void Init()
        {
            Logman.Log("SaveService - Init");
        }

        public void Release()
        {
            Logman.Log("SaveService - Release");
        }

        public void Save()
        {
         
        }

        public void DeleteSave()
        {
         
        }
    }
}

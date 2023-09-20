using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface ISaveService : IService
    {
        void Save();
        void DeleteSave();
    }
}

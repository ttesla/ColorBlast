using UnityEditor;
using UnityEngine;

namespace ColorBlast 
{
    public class SaveEditor : MonoBehaviour
    {
        [MenuItem("InsertCoin/Clear Save Data")]
        private static void ClearSaveData()
        {
            SaveFileManager.DeleteSave(SaveService.SaveFileName);
        }
    }
}


using System.IO;
using System;
using UnityEngine;
using Utility;

namespace ColorBlast 
{
    public static class SaveFileManager
    {
        public static bool WriteToFile(string fileName, string fileContent)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, fileContent);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Logman.LogWarning($"Save file not found! {fullPath} with exception {e}");
                Logman.LogWarning("Ignore this error, if it is your first time opening the game.");
                result = "";
                return false;
            }
        }

        public static bool DeleteSave(string fileName)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try 
            {
                File.Delete(fullPath);
                return true;
            }
            catch (Exception e) 
            {
                Logman.LogError($"Can't delete save file, file not found! {fullPath} with exception {e}");
                return false;
            }
        }
    }
}

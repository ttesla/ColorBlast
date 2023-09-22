using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class SaveService : ISaveService
    {
        public const string SaveFileName = "PlayerData.dat";
        private SaveData mSaveData;

        public event Action SaveLoaded;

        public void Init()
        {
            Logman.Log("SaveService - Init");
        }

        public void Release()
        {
            Logman.Log("SaveService - Release");
        }

        public bool IsReady()
        {
            return mSaveData != null;
        }

        public SaveData Load()
        {
            if (IsReady())
            {
                SaveLoaded?.Invoke();
                return mSaveData;
            }

            LoadInternal();

            // Create default save data if there is no previous one
            if (mSaveData == null)
            {
                CreateDefaultSaveData();
            }

            SaveLoaded?.Invoke();
            return mSaveData;
        }

        public void Save()
        {
            if (IsReady())
            {
                SaveInternal();
            }
        }

        public void DeleteSave()
        {
            if (IsReady())
            {
                mSaveData.ResetToDefaults();
            }
        }

        public SaveData GetSaveData() 
        {
            return mSaveData;
        }

        private void CreateDefaultSaveData()
        {
            mSaveData = new SaveData();
            mSaveData.ResetToDefaults();
            SaveInternal();
        }

        private void SaveInternal()
        {
            float startTime = Time.realtimeSinceStartup;

            SaveFileManager.WriteToFile(SaveFileName, JsonUtility.ToJson(mSaveData));

            Logman.Log("Game saved: " + (Time.realtimeSinceStartup - startTime));
        }

        private void LoadInternal()
        {
            if (SaveFileManager.LoadFromFile(SaveFileName, out string jsonStr))
            {
                mSaveData = null;

                if (!jsonStr.Equals(string.Empty))
                {
                    try
                    {
                        Logman.Log(jsonStr);
                        mSaveData = JsonUtility.FromJson<SaveData>(jsonStr);
                    }
                    catch
                    {
                        Debug.LogError("ERROR: Can't de-serialize json str, " + jsonStr);
                    }
                }
            }
        }
    }
}

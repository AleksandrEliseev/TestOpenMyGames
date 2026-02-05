using System;
using Level;
using UnityEngine;
using VContainer;

namespace SaveLoadService
{
    public class PrefsSaveLoadService : ILoadService, ISaveService, IDisposable
    {
        private const string LevelDataKey = "LevelData";

        private SaveLevelData _saveLevelData;

        [Inject]
        public PrefsSaveLoadService()
        {
            if (PlayerPrefs.HasKey(LevelDataKey))
            {
                _saveLevelData = DeserializeLevel<SaveLevelData>(PlayerPrefs.GetString(LevelDataKey));
            }
        }

        public void UpdateLevelData(SaveLevelData saveLevelData)
        {
            _saveLevelData = saveLevelData;
        }
        public void ClearLevelData()
        {
            if (PlayerPrefs.HasKey(LevelDataKey))
            {
                PlayerPrefs.DeleteKey(LevelDataKey);
            }

            _saveLevelData = null;
        }
        public int GetCurrentLevel()
        {
            return _saveLevelData == null ? 1 : _saveLevelData.LevelNumber;
        }
        public bool TryToLoadLevel(int levelNumber, out SaveLevelData levelData)
        {
            if (_saveLevelData != null && _saveLevelData.LevelNumber == levelNumber)
            {
                levelData = _saveLevelData;
                Debug.Log($"Level {levelNumber} has been loaded.");
                return true;
            }

            levelData = null;
            return false;
        }
        private string SerializeLevel<T>(T levelModel)
        {
            var json = JsonUtility.ToJson(levelModel);
            Debug.Log("Serialized Level Data: " + json);
            return json;
        }
        private T DeserializeLevel<T>(string serializedLevel)
        {
            return JsonUtility.FromJson<T>(serializedLevel);
        }
        private void SaveLevel(SaveLevelData levelModel)
        {
            string serializedLevel = SerializeLevel(levelModel);
            string key = LevelDataKey;

            PlayerPrefs.SetString(key, serializedLevel);
            PlayerPrefs.Save();
            Debug.Log(serializedLevel);
        }
        public void Dispose()
        {
            if (_saveLevelData != null)
            {
                SaveLevel(_saveLevelData);
            }

            PlayerPrefs.Save();
        }
    }

    public interface ILoadService
    {
        int GetCurrentLevel();
        bool TryToLoadLevel(int levelNumber, out SaveLevelData levelData);
    }

    public interface ISaveService
    {
        void ClearLevelData();
        void UpdateLevelData(SaveLevelData saveLevelData);
    }

    [Serializable]
    public class SaveLevelData
    {
        public int LevelNumber;
        public LevelModel LevelModel;
    }
}
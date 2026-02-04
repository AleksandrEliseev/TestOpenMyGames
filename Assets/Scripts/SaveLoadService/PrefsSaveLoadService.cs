using System;
using Level;
using UnityEngine;

namespace SaveLoadService
{
    public class PrefsSaveLoadService : ILoadService, ISaveService,IDisposable
    {
        private const string LevelKeyPrefix = "Level_";

        public void SaveLevel(int levelNumber, LevelModel levelModel)
        {
            string serializedLevel = SerializeLevel(levelModel);
            string key = LevelKeyPrefix + levelNumber;
            PlayerPrefs.SetString(key, serializedLevel);
            PlayerPrefs.Save();
        }
        public void ClearLevel(int levelNumber)
        {
            string key = LevelKeyPrefix + levelNumber;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        public bool TryToLoadLevel(int levelNumber, out LevelModel levelModel)
        {
            string key = LevelKeyPrefix + levelNumber;
            if (PlayerPrefs.HasKey(key))
            {
                string serializedLevel = PlayerPrefs.GetString(key);
                levelModel = DeserializeLevel<LevelModel>(serializedLevel);
                return true;
            }

            levelModel = default;
            return false;
        }

        private string SerializeLevel<T>(T levelModel)
        {
            return JsonUtility.ToJson(levelModel);
        }

        private T DeserializeLevel<T>(string serializedLevel)
        {
            return JsonUtility.FromJson<T>(serializedLevel);
        }
        public void Dispose()
        {
            PlayerPrefs.Save();
        }
    }

    public interface ILoadService
    {
        bool TryToLoadLevel(int levelNumber, out LevelModel levelModel);
    }

    public interface ISaveService
    {
        void SaveLevel(int levelNumber, LevelModel levelModel);
        void ClearLevel(int levelNumber);
    }
}
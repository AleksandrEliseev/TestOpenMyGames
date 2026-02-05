using UnityEngine;
using VContainer;

namespace SaveLoadService.Strategy
{
    public class PrefsSaveStrategy : ISaveStrategy
    {
        private ISerializer _serializer;

        [Inject]
        public PrefsSaveStrategy(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Save<T>(string key, T data)
        {
            string json = _serializer.Serialize(data);
            PlayerPrefs.SetString(key, json);
        }

        public T Load<T>(string key)
        {
            if (!HasKey(key))
                return default;
            
            string json = PlayerPrefs.GetString(key);
            T data = _serializer.Deserialize<T>(json);
            return data;
        }
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public void Flush()
        {
            PlayerPrefs.Save();
        }
    }
}
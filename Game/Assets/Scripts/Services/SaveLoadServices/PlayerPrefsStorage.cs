using Core.Extension;
using UnityEngine;

namespace Services.SaveLoadServices
{
    public class PlayerPrefsStorage<T> : ISaveStorage<T>
    {
        private readonly string _dataKey;

        public PlayerPrefsStorage(string dataKey)
        {
            _dataKey = dataKey;
        }

        public void Save(T data)
        {
            string json = data.ToJson();

            PlayerPrefs.SetString(_dataKey, json);
            PlayerPrefs.Save();
        }

        public T Load()
        {
            return PlayerPrefs
                .GetString(_dataKey)
                .FromJson<T>();
        }
    }
    public interface ISaveStorage<T>
    {
        public void Save(T data);

        public T Load();
    }
}
using UnityEngine;

namespace Services.SaveLoadServices
{
    public class SaveLoadService : ISaveLoadService
    {
        private PlayerProgress _playerProgress;
        private PlayerPrefsStorage<PlayerProgress> _playerPrefsStorage;
        
        private string playerProgressKey = "PlayerProgress";

        public SaveLoadService() =>
            _playerPrefsStorage = new PlayerPrefsStorage<PlayerProgress>(playerProgressKey);

        public void Save()
        {
            _playerPrefsStorage.Save(_playerProgress);
        }

        public void Load() =>
            _playerProgress = _playerPrefsStorage.Load() ?? new PlayerProgress();

        public PlayerProgress GetProgress() => 
            _playerProgress;

        public void ClearSave()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
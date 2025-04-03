using System;
using Data.LevelsData;
using GameLogic;
using GameLogic.Enemys;
using Services.SaveLoadServices;
using Services.SoundServices;
using UnityEngine;

namespace UI.EndGameUI
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] 
        private LoseGameWindow _loseGameWindow;
        [SerializeField]
        private WinnerGameWindow _winnerGameWindow;
        
        private ISaveLoadService _saveLoadService;
        private PlayerProgress _playerProgress;
        private LevelData _levelData;
        private MainBuilding _mainBuilding;
        private EnemySpawner _enemySpawner;
        private SoundService _soundService;

        public void Init(ISaveLoadService saveLoadService, LevelData levelData, MainBuilding mainBuilding, 
            EnemySpawner enemySpawner, SoundService soundService)
        {
            _saveLoadService = saveLoadService;
            _playerProgress = _saveLoadService.GetProgress();
            _levelData = levelData;
            _mainBuilding = mainBuilding;
            _enemySpawner = enemySpawner;
            _soundService = soundService;
            
            _winnerGameWindow.Init(_playerProgress, _soundService);
            _loseGameWindow.Init(_playerProgress, _levelData, _soundService);

            if (_enemySpawner == null)
                throw new Exception(" Dont registration Enemy Spawner");
            
            _mainBuilding.OnDestroyBuilding += OnDestroyBuilding;
            _enemySpawner.LevelCompleted += LevelCompleted;
        }

        private void LevelCompleted()
        {
            _soundService.PlayWinnerSfx();
            
            int nextLevel = _levelData.Level + 1;
            _playerProgress.SaveCurrentLevel(nextLevel);
            _saveLoadService.Save();
            _winnerGameWindow.gameObject.SetActive(true);
        }

        private void OnDestroyBuilding()
        {
            _soundService.PlayGameOverSfx();
            _loseGameWindow.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _mainBuilding.OnDestroyBuilding -= OnDestroyBuilding;
            _enemySpawner.LevelCompleted -= LevelCompleted;
        }
    }
}
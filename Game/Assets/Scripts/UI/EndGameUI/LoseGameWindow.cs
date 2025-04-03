using Data.LevelsData;
using Services.SaveLoadServices;
using Services.SoundServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.EndGameUI
{
    public class LoseGameWindow : MonoBehaviour
    {
        [SerializeField] 
        private Button _retryLevelBtn;
        [SerializeField] 
        private Button _QuitlBtn;

        private PlayerProgress _playerProgress;
        private LevelData _levelData;
        private SoundService _soundService;

        public void Init(PlayerProgress playerProgress, LevelData levelData, SoundService soundService)
        {
            _playerProgress = playerProgress;
            _levelData = levelData;
            _soundService = soundService;
        }

        private void Start()
        {
            _retryLevelBtn.onClick.AddListener(ClickRetryLevel);
            _QuitlBtn.onClick.AddListener(ClickQuit);
        }

        private void ClickRetryLevel()
        {
            _soundService.PlayClickSfx();
            _playerProgress.SaveCurrentLevel(_levelData.Level);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        private void ClickQuit()
        {
            _soundService.PlayClickSfx();
            SceneManager.LoadSceneAsync(0);
        }

        private void OnDestroy()
        {
            _retryLevelBtn.onClick.RemoveListener(ClickRetryLevel);
            _QuitlBtn.onClick.RemoveListener(ClickQuit);
        }

    }
}
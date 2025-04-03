using Data.LevelsData;
using Services.SaveLoadServices;
using Services.SoundServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.EndGameUI
{
    public class WinnerGameWindow : MonoBehaviour
    {
        [SerializeField] 
        private Button _nextLevelBtn;
        [SerializeField] 
        private Button _ExitBtn;

        private PlayerProgress _playerProgress;
        private SoundService _soundService;

        public void Init(PlayerProgress playerProgress, SoundService soundService)
        {
            _playerProgress = playerProgress;
            _soundService = soundService;
        }

        private void Start()
        {
            _nextLevelBtn.onClick.AddListener(ClickNextLevel);
            _ExitBtn.onClick.AddListener(ClickQuit);
        }

        private void ClickNextLevel()
        {
            _soundService.PlayClickSfx();
            SceneManager.LoadSceneAsync
                (_playerProgress.CurrentLevel <= _playerProgress.LevelsCount ? 1 : 0);
        }

        private void ClickQuit()
        {
            _soundService.PlayClickSfx();
            SceneManager.LoadSceneAsync(0);
        }

        private void OnDestroy()
        {
            _nextLevelBtn.onClick.RemoveListener(ClickNextLevel);
            _ExitBtn.onClick.RemoveListener(ClickQuit);
        }
    }
}
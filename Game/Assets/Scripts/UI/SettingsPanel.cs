using System;
using Core.Extension;
using Services.SoundServices;
using UI.MainMenuElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _soundSettingsBtn;
        [SerializeField]
        private Button _retryBtn;
        [SerializeField]
        private Button _mainMenuBtn;
        [SerializeField]
        private Button _exitBtn;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private SoundSettingsMenu _soundSettingsMenu;
        private SoundService _soundService;


        public void Init(SoundSettingsMenu soundSettingsMenu, SoundService soundService)
        {
            _soundSettingsMenu = soundSettingsMenu;
            _soundService = soundService;
        }

        private void Start()
        {
            _soundSettingsBtn.onClick.AddListener(ClickSoundSettings);
            _retryBtn.onClick.AddListener(ClickRetryLevel);
            _mainMenuBtn.onClick.AddListener(OpenMainMenu);
            _exitBtn.onClick.AddListener(HideSettingsPanel);

            _soundSettingsMenu.OnExit += Show;
        }

        private void ClickRetryLevel() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void Show() => 
            _canvasGroup.SetActive(true);

        private void HideSettingsPanel()
        {
            _soundService.PlayClickSfx();
            _canvasGroup.SetActive(false);
        }

        private void OpenMainMenu()
        {
            _soundService.PlayClickSfx();
            SceneManager.LoadScene(0);
        }

        private void ClickSoundSettings()
        {
            _soundService.PlayClickSfx();
            _soundSettingsMenu.Show();
            
            HideSettingsPanel();
        }

        private void OnDestroy()
        {
            _soundSettingsBtn.onClick.RemoveListener(ClickSoundSettings);
            _retryBtn.onClick.RemoveListener(ClickRetryLevel);
            _mainMenuBtn.onClick.RemoveListener(OpenMainMenu);
            _exitBtn.onClick.RemoveListener(HideSettingsPanel);
            
            _soundSettingsMenu.OnExit -= Show;
        }

    }
}
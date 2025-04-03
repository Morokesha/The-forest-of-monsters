using Core.Extension;
using Services.AssetServices;
using Services.FactoryServices;
using Services.SaveLoadServices;
using Services.SoundServices;
using Services.StaticDataServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenuElements
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private SoundService _soundService;
        [SerializeField]
        private SoundSettingsMenu _soundSettingsMenu;
        [SerializeField]
        private SelectLevelPanel _selectLevelPanel;

        [SerializeField] private Button _playGameBtn;
        [SerializeField] private Button _selectLevelBtn;
        [SerializeField] private Button _settingsBtn;
        
        [SerializeField] private CanvasGroup _mainMenuBtns;

        private ISaveLoadService _saveLoadService;
        private UIFactory _uiFactory;
        private AssetProvider _assetProvider;


        private void Awake()
        {
            _saveLoadService = new SaveLoadService();
            _saveLoadService.Load();
            _assetProvider = new AssetProvider();
            _uiFactory = new UIFactory(_assetProvider, _saveLoadService, _soundService);
            
            _soundService.Init(_saveLoadService,_soundSettingsMenu);
            _soundSettingsMenu.Init(_soundService);
            _selectLevelPanel.Init(_uiFactory, _saveLoadService, _soundService);
        }

        private void Start()
        {
            _settingsBtn.onClick.AddListener(ShowSettingsPanel);
            _playGameBtn.onClick.AddListener(PlayGame);
            _selectLevelBtn.onClick.AddListener(ShowSelectedLevels);
            
            _soundSettingsMenu.OnExit += SoundSettingsMenuOnExit;
            _selectLevelPanel.OnExit += SelectedLevelPanelOnExit;
        }
        private void ShowSelectedLevels()
        {
            _soundService.PlayClickSfx();
            _selectLevelPanel.Show();
            _mainMenuBtns.SetActive(false);
        }

        private void SelectedLevelPanelOnExit() =>
            _mainMenuBtns.SetActive(true);
        
        private void PlayGame()
        {
            _soundService.PlayClickSfx();
            SceneManager.LoadSceneAsync(1);
        }

        private void SoundSettingsMenuOnExit() => 
            _mainMenuBtns.SetActive(true);
        
        private void ShowSettingsPanel()
        {
            _soundService.PlayClickSfx();
            
            _soundSettingsMenu.Show();
            _mainMenuBtns.SetActive(false);
        }

        private void OnDestroy()
        {
            _settingsBtn.onClick.RemoveListener(ShowSettingsPanel);
            _playGameBtn.onClick.RemoveListener(PlayGame);
            _selectLevelBtn.onClick.RemoveListener(ShowSelectedLevels);
            
            _soundSettingsMenu.OnExit -= SoundSettingsMenuOnExit;
            _selectLevelPanel.OnExit -= SelectedLevelPanelOnExit;
        }
    }
}
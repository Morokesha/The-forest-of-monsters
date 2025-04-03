using Services.AssetServices;
using Services.ResourceRepositoryService;
using Services.SaveLoadServices;
using Services.SoundServices;
using Services.StaticDataServices;
using UI;
using UI.BuyTurretUI;
using UI.EndGameUI;
using UI.MainMenuElements;
using UI.TutorialUI;
using UI.UpgradeUI;
using UI.UIRootElements;
using UnityEngine;

namespace Services.FactoryServices
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SoundService _soundService;
        
        private UIRoot _uiRoot;

        public UIFactory(IAssetProvider assetProvider, ISaveLoadService saveLoadService, SoundService soundService)
        {
            _assetProvider = assetProvider;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
        }

        public void CreateRootUI() => 
            _uiRoot = _assetProvider.Instantiate<UIRoot>(AssetPath.UIRootPath);

        public BuyTurretMenu CreateBuyTurretMenu()
        {
            BuyTurretMenu buyTurretMenu = _assetProvider.Instantiate<BuyTurretMenu>(AssetPath.BuyTurretMenuPath);
            buyTurretMenu.transform.SetParent(_uiRoot.transform, false);

            return buyTurretMenu;
        }
        
        public UpgradeTurretMenu CreateUpgradeMenu()
        {
            UpgradeTurretMenu upgradeTurretMenu = _assetProvider.Instantiate<UpgradeTurretMenu>
                (AssetPath.UpgradeTurretMenuPath);
            upgradeTurretMenu.transform.SetParent(_uiRoot.transform, false);

            return upgradeTurretMenu;
        }

        public TutorialInfo CreateTutorialInfo()
        {
            TutorialInfo tutorialInfo = _assetProvider.Instantiate<TutorialInfo>(AssetPath.TutorialInfoPath);
            tutorialInfo.SetSaveLoadService(_saveLoadService);
            tutorialInfo.transform.SetParent(_uiRoot.transform, false);
            return tutorialInfo;
        }

        public SliderToEndLevel CreateLevelEndSlider()
        {
            SliderToEndLevel sliderToEndLevel = _assetProvider.Instantiate<SliderToEndLevel>
                (AssetPath.SliderToEndLevelPath);
            sliderToEndLevel.transform.SetParent(_uiRoot.transform, false);
            
            return sliderToEndLevel;
        }

        public EndGameWindow CreateEndGameWindow()
        {
            EndGameWindow endGameWindow = _assetProvider.Instantiate<EndGameWindow>
                (AssetPath.EndGameWindowPath);
            endGameWindow.transform.SetParent(_uiRoot.transform, false);
            
            return endGameWindow;
        }

        public void CreateGoldCounterUI(ResourceRepository resourceRepository)
        {
            GoldCounterUI goldCounterUI = _assetProvider.Instantiate<GoldCounterUI>(AssetPath.GoldCounterPath);
            goldCounterUI.transform.SetParent(_uiRoot.transform, false);
            goldCounterUI.Init(resourceRepository);
        }

        public SoundSettingsMenu CreateSoundSettingsMenu()
        {
            SoundSettingsMenu soundSettingsMenu =
                _assetProvider.Instantiate<SoundSettingsMenu>(AssetPath.SoundSettingsMenuPath);
            soundSettingsMenu.transform.SetParent(_uiRoot.transform, false);

            return soundSettingsMenu;
        }
        
        public SettingsPanel CreateSettingsPanel()
        {
            SettingsPanel settingsPanel =
                _assetProvider.Instantiate<SettingsPanel>(AssetPath.SettingsMenuPath);
            _uiRoot.SetPreferences(settingsPanel, _soundService);
            settingsPanel.transform.SetParent(_uiRoot.transform, false);

            return settingsPanel;
        }

        public SelectLevelBtn CreateSelectLevelButton(Transform parent)
        {
            SelectLevelBtn selectLevelBtn = _assetProvider.Instantiate<SelectLevelBtn>(AssetPath.SelectLevelBtn);
            selectLevelBtn.transform.SetParent(parent, false);
            return selectLevelBtn;
        }
    }
}
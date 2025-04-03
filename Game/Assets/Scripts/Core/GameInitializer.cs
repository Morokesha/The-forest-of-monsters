using Data.LevelsData;
using GameLogic;
using GameLogic.Enemys;
using GameLogic.Turrets;
using GameLogic.Turrets.TurretSpawn;
using Services.AssetServices;
using Services.FactoryServices;
using Services.ResourceRepositoryService;
using Services.SaveLoadServices;
using Services.SoundServices;
using Services.StaticDataServices;
using UI;
using UI.EndGameUI;
using UI.MainMenuElements;
using UI.UIRootElements;
using UnityEngine;

namespace Core
{
    public class GameInitializer
    {
        private IFactoryService _factoryService;
        private IStaticData _staticData;
        private IAssetProvider _assetProvider;
        private IUIFactory _uiFactory;
        private ISaveLoadService _saveLoadService;
        private ResourceRepository _resourceRepository;
        
        private TurretSpawner _turretSpawner;
        private EnemySpawner _enemySpawner;
        private GameObject _mainBuildingPoint;
        private SliderToEndLevel _sliderToEndLevel;
        private EndGameWindow _endGameWindow;
        private SoundSettingsMenu _soundSettingsMenu;
        private SoundService _soundService;
        private SettingsPanel _settingsPanel;

        public void Init()
        {
            InitServices();
            InitUI();
            InitWorld();
        }

        private void InitServices()
        {
            _saveLoadService = new SaveLoadService();
            _saveLoadService.Load();
            
            _assetProvider = new AssetProvider();
            _staticData = new StaticData(_assetProvider);
            _staticData.Init();
            _factoryService = new FactoryService(_assetProvider, _staticData);
            _soundService = _factoryService.CreateSoundService();
            _resourceRepository = new ResourceRepository();
            _uiFactory = new UIFactory(_assetProvider, _saveLoadService, _soundService);
        }
        private void InitUI()
        {
            _uiFactory.CreateRootUI();
            _sliderToEndLevel = _uiFactory.CreateLevelEndSlider();
            _endGameWindow = _uiFactory.CreateEndGameWindow();
            _uiFactory.CreateGoldCounterUI(_resourceRepository);
            _soundSettingsMenu = _uiFactory.CreateSoundSettingsMenu();
            _settingsPanel = _uiFactory.CreateSettingsPanel();
        }

        private void InitWorld()
        {
            LevelData levelData = _staticData.GetLevelData(_saveLoadService.GetProgress().CurrentLevel);
            if (levelData == null) 
                Debug.Log("Level Data not instance");
            
            _factoryService.CreateLevel(levelData);
            
            _mainBuildingPoint = GameObject.FindGameObjectWithTag("MainBuilding");
            
            _turretSpawner = Object.FindObjectOfType<TurretSpawner>();
            _enemySpawner = Object.FindObjectOfType<EnemySpawner>();
            _soundService.Init(_saveLoadService, _soundSettingsMenu);
            
            MainBuilding mainBuilding = _factoryService.CreateMainBuilding(_mainBuildingPoint.transform);
            _turretSpawner.Init(_factoryService, _uiFactory, _staticData,
                _resourceRepository, mainBuilding, _soundService, _saveLoadService.GetProgress().CurrentLevel);
            _enemySpawner.Init(_factoryService, _staticData, _saveLoadService.GetProgress(), _resourceRepository, 
                _mainBuildingPoint.transform);
            
            _sliderToEndLevel.Init(_enemySpawner);
            _endGameWindow.Init(_saveLoadService, levelData, mainBuilding, _enemySpawner, _soundService);
            _settingsPanel.Init(_soundSettingsMenu, _soundService);
            _soundSettingsMenu.Init(_soundService);
            
            _resourceRepository.AddGold(levelData.StartGoldOnLevel);
        }

    }
}
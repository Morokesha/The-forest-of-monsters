using System;
using Data;
using Data.Enums;
using Data.TurretsData;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.FactoryServices;
using Services.ResourceRepositoryService;
using Services.SoundServices;
using Services.StaticDataServices;
using UI;
using UI.BuyTurretUI;
using UI.TutorialUI;
using UI.UpgradeUI;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

namespace GameLogic.Turrets.TurretSpawn
{
    public class TurretSpawner : MonoBehaviour
    {
        public event Action OnShopOpened;
        public event Action OnTurretPurchased;
        public event Action OnTurretSelected;
        
        [SerializeField]
        private ProjectilePool _projectilePool;
        [SerializeField]
        private LayerMask _spawnPointMask;
        
        
        private ResourceRepository _resourceRepository;
        private IFactoryService _factoryService;
        private IUIFactory _uiFactoryService;
        private IStaticData _staticData;

        private TurretSpawnPoint _spawnPoint;
        private BuyTurretMenu _buyTurretMenu;
        private UpgradeTurretMenu _upgradeTurretMenu;
        private MainBuilding _mainBuilding;
        private SoundService _soundService;
        
        private Turret _instanceTurret;
        private Turret _selectTurret;

        private int _currentGameLevel;
        private int _fingerID = -1;
        
        private bool _isClickOnUI = false;
        
        public void Init(IFactoryService factoryService, IUIFactory uiFactory, IStaticData staticData,
            ResourceRepository resourceRepository, MainBuilding mainBuilding, SoundService soundService, 
            int currentGameLevel)
        {
            _factoryService = factoryService;
            _uiFactoryService = uiFactory;
            _staticData = staticData;
            _resourceRepository = resourceRepository;
            _mainBuilding = mainBuilding;
            _soundService = soundService;
            _currentGameLevel = currentGameLevel;
            
            _projectilePool.Init(_factoryService, transform.position);

            _mainBuilding.OnDestroyBuilding += GameOver;
            
            CreatedInteractiveMenu();
        }

        private void Awake()
        {
            if (YandexGame.EnvironmentData.isMobile)
            {
#if UNITY_WEBGL
       _fingerID = 0;     
#endif
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (YandexGame.EnvironmentData.isDesktop)
                {
                    _isClickOnUI = EventSystem.current.IsPointerOverGameObject();
                    
                    if (_isClickOnUI)
                        return;
                }

                if (YandexGame.EnvironmentData.isMobile)
                {
                    if (EventSystem.current.IsPointerOverGameObject(_fingerID))
                    {
                        //GUI Action
                        return;
                    }
                }

                if (GetSpawnPoint() != null)
                {
                    _buyTurretMenu.SetDeactivated();
                    _upgradeTurretMenu.DeactivatedMenu();
                    if (_selectTurret != null)
                        _selectTurret.HideRadius();

                    _spawnPoint = GetSpawnPoint();
                    _selectTurret = _spawnPoint.GetActiveTurret();

                    if (_spawnPoint.IsFree())
                        BuyMenuActivated();
                    else
                        UpgradeMenuActivated();
                }

                else if (GetSpawnPoint() != _spawnPoint || GetSpawnPoint() == null)
                    if (_buyTurretMenu != null && _upgradeTurretMenu != null)
                        DeactivatedAll();
            }
        }

        private void DeactivatedAll()
        {
            _buyTurretMenu.SetDeactivated();
            _upgradeTurretMenu.DeactivatedMenu();
            if (_selectTurret != null)
                _selectTurret.HideRadius();
        }

        private void BuyMenuActivated()
        {
            _buyTurretMenu.SetActivated();
            OnShopOpened?.Invoke();
        }

        private void UpgradeMenuActivated()
        {
            _upgradeTurretMenu.SetPreferences(_staticData.GetTurretData(_spawnPoint.GetActiveTurret().
                GetTurretData().TurretType, _spawnPoint.GetActiveTurret().
                GetTurretData().NextUpgradeStage), _spawnPoint.GetActiveTurret().GetTurretData(),
                _soundService);

            _selectTurret.ShowRadius();
            
            _upgradeTurretMenu.ActivatedMenu();
            
            OnTurretSelected?.Invoke();
        }
        
        private void GameOver() => 
            DeactivatedAll();


        private void CreatedInteractiveMenu()
        {
            InstanceBuyMenu();
            InstanceUpgradeTurretMenu();

            if (_currentGameLevel == 1)
            { 
                TutorialInfo tutorialInfo = _uiFactoryService.CreateTutorialInfo(); 
                tutorialInfo.Init(this, _buyTurretMenu, _upgradeTurretMenu);
            }
        }

        private void InstanceUpgradeTurretMenu()
        {
            _upgradeTurretMenu = _uiFactoryService.CreateUpgradeMenu();
            _upgradeTurretMenu.OnCancerReceived += CancerReceived;
            _upgradeTurretMenu.OnClickUpgrade += OnClickUpgrade;
            _upgradeTurretMenu.OnClickRemove += OnClickRemoveTurret;
            _upgradeTurretMenu.OnClickExit += DeactivatedAll;
            _upgradeTurretMenu.DeactivatedMenu();
        }

        private void CancerReceived()
        {
            if (_selectTurret != null)
                _selectTurret.HideRadius();
        }

        private void InstanceBuyMenu()
        {
            _buyTurretMenu = _uiFactoryService.CreateBuyTurretMenu();
            _buyTurretMenu.Init(_staticData, _soundService);
            _buyTurretMenu.PurchasedTurret += OnPurchasedTurret;
            _buyTurretMenu.OnClickExit += DeactivatedAll;
            _buyTurretMenu.SetDeactivated();
        }

        private void OnClickUpgrade(TurretData turretData)
        {
            TurretData upgradeData = _staticData.GetTurretData(turretData.TurretType, turretData.UpgradeStage);
            int price = upgradeData.Price;
            
            if (_resourceRepository.GetGold() >= price)
            {
                Turret newTurret = _factoryService.CreateTurret(transform, _spawnPoint.transform,
                    upgradeData, _projectilePool);
                _spawnPoint.DestroyTurretOnPoint();
                _spawnPoint.SetFreeState(false, newTurret);
                newTurret.HideRadius();
                _resourceRepository.SpentGold(price);
            }

            if (_selectTurret != null) 
                _selectTurret.HideRadius();
        }
        
        private void OnPurchasedTurret(TurretData turretData)
        {
            if (_resourceRepository.GetGold() >= turretData.Price)
            {
                _resourceRepository.SpentGold(turretData.Price);
                SpawnDefaultTurret(_spawnPoint.transform, turretData.TurretType);
                _spawnPoint.SetFreeState(false, _instanceTurret);
                _buyTurretMenu.SetDeactivated();
                OnTurretPurchased?.Invoke();
            }
        }
        
        private void OnClickRemoveTurret(TurretData turretData)
        {
            _resourceRepository.AddGold(turretData.Price/2);
            _spawnPoint.DestroyTurretOnPoint();
        }


        private void SpawnDefaultTurret(Transform pos, TurretType turretType) =>
            _instanceTurret = _factoryService.CreateTurret(transform, pos, _staticData.GetTurretData
                (turretType, UpgradeStage.Stage1), _projectilePool);

        private TurretSpawnPoint GetSpawnPoint()
        {
            TurretSpawnPoint turretSpawnPoint = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out var hits, 100f,
                _spawnPointMask))
            {
                if (hits.collider.TryGetComponent(out TurretSpawnPoint spawnPoint))
                    turretSpawnPoint = spawnPoint;
            }
            
            return turretSpawnPoint;
        }

        private void OnDestroy()
        {
            if (_buyTurretMenu != null)
            {
                _buyTurretMenu.PurchasedTurret -= OnPurchasedTurret;
                _buyTurretMenu.OnClickExit -= DeactivatedAll;
            }
            
            if (_upgradeTurretMenu != null)
            {
                _upgradeTurretMenu.OnClickUpgrade -= OnClickUpgrade;
                _upgradeTurretMenu.OnClickRemove -= OnClickRemoveTurret;
                _upgradeTurretMenu.OnCancerReceived -= CancerReceived;
                _upgradeTurretMenu.OnClickExit -= DeactivatedAll;
            }

            _mainBuilding.OnDestroyBuilding -= GameOver;
        }
    }
}
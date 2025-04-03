using System;
using Data.Enums;
using Data.TurretsData;
using Services.SoundServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeUI
{
    public class UpgradeTurretMenu : MonoBehaviour
    {
        public event Action OnSelectUpgrade;
        public event Action OnCancerReceived;
        public event Action OnClickExit; 
        
        public event Action<TurretData> OnClickUpgrade;
        public event Action<TurretData> OnClickRemove; 
        
        [SerializeField] 
        private Button _upgradeBtn;
        [SerializeField]
        private Button _removeMenuBtn;
        [SerializeField]
        private Button _closeMenuBtn;
        
        [SerializeField] 
        private UpgradeTurretBtn _upgradeTurret;
        [SerializeField] 
        private RemoveTurretMenu _removeTurretMenu;
        [SerializeField] 
        private DescriptionItem _descriptionItem;
        [SerializeField]
        private GameObject _priceContainer;
        [SerializeField]
        private GameObject _buttonsContainer;
        
        private TurretData _upgradeTurretData;
        private SoundService _soundService;
        private TurretData _currentData;

        private void OnEnable()
        {
            _upgradeBtn.onClick.AddListener(ClickUpgradeMenu);
            _removeMenuBtn.onClick.AddListener(ClickRemoveMenu);
            _closeMenuBtn.onClick.AddListener(CloseMenu);
            
            _removeTurretMenu.OnRemoveTurret += ClickRemoveMenu;
            
            _descriptionItem.gameObject.SetActive(false);
            _priceContainer.SetActive(false);
        }

        private void CloseMenu() =>
            OnClickExit?.Invoke();

        public void SetPreferences(TurretData upgradeTurretData, TurretData currentData, SoundService soundService)
        {
            _upgradeTurretData = upgradeTurretData;
            _currentData = currentData;
            _soundService = soundService;
            
            _removeTurretMenu.SetSellCost(_currentData.Price/2);
        }

        public void ActivatedMenu()
        {
            if (_upgradeTurretData.UpgradeStage == UpgradeStage.Stage1)
                _upgradeTurret.Deactivated();
            if (_upgradeTurretData != null && _upgradeTurretData.UpgradeStage != UpgradeStage.Stage1)
            {
                _upgradeTurret.Activated();
                _upgradeTurret.UpdateButtonVisual(_upgradeTurretData);
            }
            
            gameObject.SetActive(true);
            _buttonsContainer.SetActive(true);
        }
        
        public void DeactivatedMenu()
        {
            gameObject.SetActive(false);
            _removeTurretMenu.gameObject.SetActive(false);
        }

        private void ClickRemoveMenu()
        {
            _soundService.PlayClickSfx();
            
            _buttonsContainer.SetActive(false);
            
            _removeTurretMenu.gameObject.SetActive(true);
            _removeTurretMenu.OnRemoveTurret += OnRemoveTurret;
            _removeTurretMenu.OnCancer += OnCancer;
        }

        private void OnCancer()
        {
            _soundService.PlayClickSfx();
            DeactivatedMenu();
            OnCancerReceived?.Invoke();
        }

        private void OnRemoveTurret()
        {
            _soundService.PlaySellTowerSfx();
            DeactivatedMenu();
            _removeTurretMenu.gameObject.SetActive(false);
            OnClickRemove?.Invoke(_currentData);
        }

        private void ClickUpgradeMenu()
        {
            _soundService.PlayClickSfx();
            _buttonsContainer.SetActive(false);
            
            _descriptionItem.SetTurretData(_upgradeTurretData);
            _descriptionItem.gameObject.SetActive(true);
            _descriptionItem.OnBuyTurret += OnBuyTurret;
            
            _priceContainer.SetActive(true);
            
            OnSelectUpgrade?.Invoke();
        }

        private void OnBuyTurret(TurretData turretData)
        {
            _soundService.PlayTowerLvlUpSfx();
            
            OnClickUpgrade?.Invoke(turretData);
            DeactivatedMenu();
        }
        
        private void OnDisable()
        {
            _upgradeBtn.onClick.RemoveListener(ClickUpgradeMenu);
            _removeMenuBtn.onClick.RemoveListener(ClickRemoveMenu);
            _closeMenuBtn.onClick.RemoveListener(CloseMenu);
            
            _removeTurretMenu.OnRemoveTurret -= ClickRemoveMenu;
            _removeTurretMenu.OnCancer -= OnCancer;
            _descriptionItem.OnBuyTurret -= OnBuyTurret;
            
            _descriptionItem.gameObject.SetActive(false);
            _priceContainer.SetActive(false);
        }

    }
}
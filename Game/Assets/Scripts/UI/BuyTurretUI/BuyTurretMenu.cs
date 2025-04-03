using System;
using System.Collections.Generic;
using Core.Extension;
using Data.Enums;
using Data.TurretsData;
using Services.SoundServices;
using Services.StaticDataServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuyTurretUI
{
    public class BuyTurretMenu : MonoBehaviour
    {
        public event Action<TurretData> PurchasedTurret;
        public event Action OnTurretSelection;
        public event Action OnClickExit;

        [SerializeField]
        private Button _closeMenuBtn;
        [SerializeField] 
        private GameObject _containerBtn;
        [SerializeField]
        private List<SelectTurretButton> _selectTurretBtns;
        [SerializeField] 
        private DescriptionItem _descriptionItem;
        
        private IStaticData _staticData;
        private SoundService _soundService;

        public void Init(IStaticData staticData, SoundService soundService)
        {
            _staticData = staticData;
            _soundService = soundService;
        }

        private void OnEnable()
        {
            _descriptionItem.OnBuyTurret += OnBuyTurret;
        }

        private void Start()
        {
            InitializeButtons();
            _closeMenuBtn.onClick.AddListener(CloseMenu);
        }

        private void CloseMenu()
        {
            OnClickExit?.Invoke();
        }

        public void SetActivated()
        {
            gameObject.SetActive(true);
            _containerBtn.SetActive(true);
        }

        public void SetDeactivated()
        {
            gameObject.SetActive(false);
            _descriptionItem.gameObject.SetActive(false);
        }
        
        private void OnSelectTurret(TurretData turretData)
        {
            _descriptionItem.gameObject.SetActive(true);
            
            _descriptionItem.SetTurretData(turretData);
            _containerBtn.SetActive(false);
            
            OnTurretSelection?.Invoke();
        }

        private void OnBuyTurret(TurretData turretData)
        {
            _soundService.PlayBuySfx();
            PurchasedTurret?.Invoke(turretData);
        }

        private void InitializeButtons()
        {
            print(_staticData.GetDefaultTurretList().Count);
            
            for (int i = 0; i < _staticData.GetDefaultTurretList().Count; i++)
            {
                _selectTurretBtns[i].Init(_staticData.GetDefaultTurretList()[i], _soundService); 
                    
                _selectTurretBtns[i].OnSelectTurret += OnSelectTurret;
                
            }
        }

        private void OnDisable()
        {
            _descriptionItem.OnBuyTurret -= OnBuyTurret;
        }

        private void OnDestroy()
        {
            foreach (var turretButton in _selectTurretBtns) 
                turretButton.OnSelectTurret -= OnSelectTurret;
        }
    }
}
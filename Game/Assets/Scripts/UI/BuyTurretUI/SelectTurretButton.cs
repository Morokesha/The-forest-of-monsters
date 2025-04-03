using System;
using Data.Enums;
using Data.TurretsData;
using Services.SoundServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuyTurretUI
{
    public class SelectTurretButton : MonoBehaviour
    {
        public event Action<TurretData> OnSelectTurret;
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _iconTurret;
        
        private TurretData _turretData;
        private SoundService _soundService;

        public void Init(TurretData turretData, SoundService soundService)
        {
            _turretData = turretData;
            _soundService = soundService;
            
            _button.onClick.AddListener(ClickSelect);
            
            SetField();
        }

        private void ClickSelect()
        {
            _soundService.PlayClickSfx();
            OnSelectTurret?.Invoke(_turretData);
        }

        private void SetField() => 
            _iconTurret.sprite = _turretData.TurretPreferences.SpriteTurret;

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ClickSelect);
        }
    }
}
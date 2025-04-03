using System;
using Data.TurretsData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class DescriptionItem : MonoBehaviour
    {
        public event Action<TurretData> OnBuyTurret; 
        
        [SerializeField]
        private TextMeshProUGUI _nameitem;
        [SerializeField] 
        private TextMeshProUGUI _description;
        [SerializeField]
        private TextMeshProUGUI _damageText;
        [SerializeField]
        private TextMeshProUGUI _radiusText;
        [SerializeField]
        private TextMeshProUGUI _reloadTxt;
        
        [SerializeField] 
        private GameObject _priceContainer;
        [SerializeField]
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private Button _buyTurretBtn;

        private TurretData _turretData;
        
        private string _damageTurret;
        private string _radiusTurret;
        private string _reload;

        public void SetTurretData(TurretData turretData)
        {
            _turretData = turretData;
            SetDescription();
        }
        
        private void Start()
        {
            _priceContainer.SetActive(true);
            _buyTurretBtn.onClick.AddListener(BuyTurret);
        }

        private void OnEnable() => 
            _priceContainer.SetActive(true);

        private void OnDisable() => 
            _priceContainer.SetActive(false);

        private void BuyTurret()
        {
            OnBuyTurret?.Invoke(_turretData);
        }

        private void SetDescription()
        {
            if (YandexGame.EnvironmentData.language == "ru")
            {
                _damageTurret = "Урон: - " + _turretData.TurretPreferences.Damage;
                _radiusTurret = "Радиус: - " + _turretData.TurretPreferences.AttackRadius;
                _reload = "Перезарядка: - " + _turretData.TurretPreferences.Reload;
                
                _nameitem.SetText(_turretData.TurretName);
                _description.SetText(_turretData.Description);
            }

            else
            {
                _damageTurret = "Damage: - " + _turretData.TurretPreferences.Damage;
                _radiusTurret = "Radius: - " + _turretData.TurretPreferences.AttackRadius;
                _reload = "Reload: - " + _turretData.TurretPreferences.Reload;
                
                _nameitem.SetText(_turretData.TurretNameEn);
                _description.SetText(_turretData.DescriptionEn);
            }

            _damageText.SetText(_damageTurret);
           _radiusText.SetText(_radiusTurret);
           _reloadTxt.SetText(_reload);
           
           _priceText.SetText(_turretData.Price.ToString());
        }
    }
}
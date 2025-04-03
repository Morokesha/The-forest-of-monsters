using System;
using Data.TurretsData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.UpgradeUI
{
    public class RemoveTurretMenu : MonoBehaviour
    {
        public event Action OnCancer;
        public event Action OnRemoveTurret; 
        
        [SerializeField]
        private Button _removeBtn;
        [SerializeField]
        private Button _cancerBtn;

        [SerializeField] 
        private TextMeshProUGUI _sellCostTxt;

        private int _sellCost;


        public void SetSellCost(int cost) =>
            _sellCost = cost;

        private void OnEnable()
        {
            _removeBtn.onClick.AddListener(RemoveTurret);
            _cancerBtn.onClick.AddListener(Cancer);

            if (YandexGame.EnvironmentData.language == "ru") 
                _sellCostTxt.SetText("Будет получено " + _sellCost);
            else
                _sellCostTxt.SetText("Will be received " + _sellCost);
        }
        private void OnDisable()
        {
            _removeBtn.onClick.RemoveListener(RemoveTurret);
            _cancerBtn.onClick.RemoveListener(Cancer);
        }

        private void RemoveTurret() =>
            OnRemoveTurret?.Invoke();

        private void Cancer() =>
            OnCancer?.Invoke();

    }
}
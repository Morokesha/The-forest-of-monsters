using Core.Extension;
using Data.TurretsData;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeUI
{
    public class UpgradeTurretBtn : MonoBehaviour
    {
        [SerializeField] 
        private Image _turretImage;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public void UpdateButtonVisual(TurretData turretData) =>
            _turretImage.sprite = turretData.TurretPreferences.SpriteTurret;

        public void Deactivated() => 
            _canvasGroup.SetActive(false);

        public void Activated() =>
            _canvasGroup.SetActive(true);
    }
}
using GameLogic.Enemys;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIRootElements
{
    public class SliderToEndLevel : MonoBehaviour
    {
        [SerializeField]
        private Image _slider;
        
        private EnemySpawner _enemySpawner;
        
        private float _maxEnemyAmount;

        public void Init(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;

            _enemySpawner.EnemyAmountChanged += UpdateSlider;
        }

        private void UpdateSlider(float currentDeathEnemy)
        {
            _maxEnemyAmount = _enemySpawner.GetEnemyAmount();
            _slider.fillAmount = currentDeathEnemy / _maxEnemyAmount;
        }

        private void OnDestroy()
        {
            _enemySpawner.EnemyAmountChanged -= UpdateSlider;
        }
    }
}
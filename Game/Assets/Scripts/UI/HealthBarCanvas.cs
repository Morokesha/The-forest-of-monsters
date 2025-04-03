using System;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBarCanvas : MonoBehaviour
    {
        [SerializeField]
        private Image _healthBarImage;
        
        private float _maxHealth;
        private float _currentHealth;
        
        private Camera _camera;
        private HealthSystem _healthSystem;

        public void Init(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            
            SetHealth();
        }

        private void Start()
        {
            _camera = Camera.main;

            GetComponent<Canvas>().worldCamera = _camera;
        }

        private void SetHealth()
        {
            _maxHealth = _healthSystem.MaxHealth;
            _healthSystem.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int amount)
        {
            _currentHealth = amount;
            UpdateHealthBar();
        }

        private void Update() =>
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);

        private void UpdateHealthBar() =>
            _healthBarImage.fillAmount = _currentHealth / _maxHealth;

        private void OnDisable()
        {
            if (_healthSystem != null) 
                _healthSystem.OnHealthChanged -= OnHealthChanged;
        }
    }
}
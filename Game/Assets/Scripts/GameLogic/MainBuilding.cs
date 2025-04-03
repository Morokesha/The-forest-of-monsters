using System;
using GameLogic.Turrets;
using Services;
using Services.SoundServices;
using UI;
using UnityEngine;

namespace GameLogic
{
    public class MainBuilding : MonoBehaviour
    {
        public event Action OnDestroyBuilding;
        
        [SerializeField]
        private HealthBarCanvas _healthBarCanvas;
        [SerializeField] 
        private ParticleSystem _hitBuildingFX;
        [SerializeField]
        private AudioSource _audioSource;
        
        private HealthSystem _healthSystem;
        private HealthBarCanvas _healthBar;

        private int _maxHealthBuilding = 150;
        
        private void Awake()
        {
            _healthSystem = new HealthSystem();
            _healthSystem.SetMaxHealth(_maxHealthBuilding);
            
            _healthBarCanvas.Init(_healthSystem);
        }

        private void Start()
        {
            _healthSystem.OnDeath += OnDeath;
        }
        
        private void OnDeath()
        {
            Turret[] turrets = FindObjectsOfType<Turret>();

            foreach (var turret in turrets) 
                turret.StopGame();
            
            OnDestroyBuilding?.Invoke();
           Destroy(gameObject, 0.1f);
        }

        public void TakeDamage(int damage)
        {
            _hitBuildingFX.Play();
            _audioSource.Play();
            _healthSystem.TakeDamage(damage);
        }

        private void OnDestroy()
        {
            _healthSystem.OnDeath -= OnDeath;
        }
    }
}
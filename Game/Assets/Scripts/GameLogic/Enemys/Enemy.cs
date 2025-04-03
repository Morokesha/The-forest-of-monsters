using System;
using System.Collections;
using System.Timers;
using Data;
using Data.Enums;
using Services;
using Services.ResourceRepositoryService;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace GameLogic.Enemys
{
    public class Enemy : MonoBehaviour
    {
        public event Action<EnemyType, Enemy> OnDead;

        public EnemyType EnemyType;

        [SerializeField] 
        private ParticleSystem _hitFX;
        [SerializeField] 
        private EnemyMovement _enemyMovement;
        [SerializeField] 
        private HealthBarCanvas _healthBarCanvas;
        [SerializeField] 
        private NavMeshAgent _agent;

        private ResourceRepository _resourceRepository;
        private EnemySpawner _enemySpawner;
        private MainBuilding _mainBuilding;
        private EnemyData _enemyData;
        private HealthSystem _healthSystem;

        private float _timer;
        private readonly float _timerToDisableFx = 0.3f;
        
        private bool _isDead;
        private bool _disableHitFx;

        public void Init(ResourceRepository resourceRepository, EnemyData enemyData, EnemySpawner enemySpawner,
            MainBuilding mainBuilding)
        {
            _resourceRepository = resourceRepository;
            _enemyData = enemyData;
            _mainBuilding = mainBuilding;
            _enemySpawner = enemySpawner;

            EnemyType = _enemyData.EnemyType;
            _healthSystem = new HealthSystem();

            _enemyMovement.Init(_enemyData, _mainBuilding, _enemySpawner, _agent);

            _timer = _timerToDisableFx;
        }

        private void OnEnable()
        {
            if (_healthSystem != null) 
                SetHealthSystem();
        }

        private void Update()
        {
            if (_disableHitFx == true)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _hitFX.Stop();
                    _disableHitFx = false;
                }
            }
        }

        private void SetHealthSystem()
        {
            _healthSystem.SetMaxHealth(_enemyData.HealthData.Health); 
            _healthBarCanvas.Init(_healthSystem);
                
            _healthSystem.OnDeath += OnDeath;

            _isDead = false;
        }

        public void MoveToTarget(Vector3 targetMove) => 
            _enemyMovement.SetTargetMove(targetMove);

        public void TakeDamage(int amount)
        {
            _healthSystem.TakeDamage(amount);
            _hitFX.Play();

            _disableHitFx = true;
            _timer = _timerToDisableFx;
        }

        private void CollisionToMainBuilding()
        {
             _enemySpawner.UpdateCurrentDeathEnemy();
             OnDead?.Invoke(EnemyType, this);
        }

        public void SetPosition(Vector3 position)
        {
            _agent.enabled = false;
            transform.position = position;
            _agent.enabled = true;
        }
        
        private void OnDeath()
        {
            _isDead = true;
            
            if (_isDead)
                _resourceRepository.AddGold(_enemyData.EnemyLootGold);
            
            _enemySpawner.UpdateCurrentDeathEnemy();
            
            OnDead?.Invoke(EnemyType, this);
        }

        private void OnDisable()
        {
            if (_healthSystem != null)
            {
                _healthSystem.OnDeath -= OnDeath;
                _healthSystem.SetMaxHealth(_enemyData.HealthData.Health); 
                _healthBarCanvas.Init(_healthSystem);                
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MainBuilding building))
            {
                building.TakeDamage(_enemyData.Damage);
                
                CollisionToMainBuilding();
            }
        }
    }
}
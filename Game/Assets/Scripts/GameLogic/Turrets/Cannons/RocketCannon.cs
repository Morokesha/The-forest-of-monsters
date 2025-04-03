using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Enums;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets.Projectiles;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.FactoryServices;
using UnityEngine;

namespace GameLogic.Turrets.Cannons
{
    public class RocketCannon : Cannon
    {
        [SerializeField] 
        private List<GameObject> _spawnPoints;
        [SerializeField]
        private List<GameObject> _rocketVisualList;
        [SerializeField]
        private AudioSource _audioSource;
        
        private ProjectilePool _projectilePool;
        private ProjectileType _projectileType;
        private TurretData _turretData;
        private Enemy _target;
        
        private float _currentReload;
        private float _reloadTime;

        public override void Init(ProjectilePool projectilePool, TurretData turretData)
        {
            _projectilePool = projectilePool;
            _turretData = turretData;
            _projectileType = _turretData.ProjectileType;
            _reloadTime = _turretData.TurretPreferences.Reload;
            
            SetPitch(_audioSource);
        }
        private void Update()
        {
            if (_target != null && _target.gameObject.activeSelf)
                AttackEnemy();
        }
        protected override void Fire()
        {
            foreach (var rocket in _rocketVisualList) 
                rocket.SetActive(true);
            StartCoroutine(SpawnRocketCo());
        }

        public override void StopAttack()
        {
            _target = null;
            StopAllCoroutines();
        }
        
        private IEnumerator SpawnRocketCo()
        {
            float interval = _reloadTime / _spawnPoints.Count;

            float timer = _reloadTime;
            int capacity = _spawnPoints.Count - 1;

            while (timer >= 0 && capacity >= 0 && _target.gameObject.activeSelf)
            {
                yield return new WaitForSeconds(interval);
                Projectile projectile =
                    _projectilePool.GetProjectile(_projectileType, _spawnPoints[capacity].transform.position);

                if (projectile == null)
                {
                    _projectilePool.CreateProjectileForType(_projectileType);
                    projectile =
                        _projectilePool.GetProjectile(_projectileType, _spawnPoints[capacity].transform.position);
                }
                
                _rocketVisualList[capacity].SetActive(false);
                projectile.SetTarget(_target);
                projectile.SetDamage(_turretData.TurretPreferences.Damage); 
                _audioSource.Play();
                
                capacity -= 1;
                timer -= interval;
            }
        }
        
        private void AttackEnemy()
        {
            _currentReload -= Time.deltaTime;
            
            if (_currentReload <= 0)
            { 
                Fire();
                _currentReload = _reloadTime * 1.5f;
            }
        }

        public override void SetTarget(Enemy target)
        {
            _target = target;
        }

        public override List<GameObject> GetSpawnPoints()
        {
            return _spawnPoints;
        }
    }
}
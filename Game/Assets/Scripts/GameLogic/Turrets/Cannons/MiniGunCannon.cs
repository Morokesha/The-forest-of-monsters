using System.Collections;
using System.Collections.Generic;
using Data.Enums;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets.Projectiles;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using UnityEngine;

namespace GameLogic.Turrets.Cannons
{
    public class MiniGunCannon : Cannon
    {
        [SerializeField] 
        private ParticleSystem _muzzleFlashFx;
        [SerializeField] 
        private List<GameObject> _spawnPoints;
        [SerializeField]
        private AudioSource _audioSource;
        
        private ProjectileType _projectileType;
        private TurretData _turretData;
        private Enemy _target;
        
        private float _currentReload;
        private float _reloadTime;
        private ProjectilePool _projectilePool;

        public override void Init(ProjectilePool projectilePool, TurretData turretData)
        {
            _projectilePool = projectilePool;
            _turretData = turretData;
            _projectileType = turretData.ProjectileType;
            _reloadTime = turretData.TurretPreferences.Reload * 4;
            
            SetPitch(_audioSource);
        }

        private void Update()
        {
            if (_target != null && _target.gameObject.activeSelf) 
                AttackEnemy();
            else
            {
                _muzzleFlashFx.Stop();
                _audioSource.Stop();
            }
        }

        protected override void Fire()
        {
            StartCoroutine(SpawnBulletCo());
        }

        public override void StopAttack()
        {
            _target = null;
            StopAllCoroutines();
        }

        private IEnumerator SpawnBulletCo()
        {
            float interval = _turretData.TurretPreferences.Reload;

            float timer = _reloadTime;
            int capacity = _spawnPoints.Count - 1;

            while (timer >= 0 && capacity >= 0)
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
                
                projectile.SetTarget(_target);
                projectile.SetDamage(_turretData.TurretPreferences.Damage); 
                _muzzleFlashFx.Play();
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
                _currentReload = _reloadTime;
            }
        }
        
        public override void SetTarget(Enemy target) => 
            _target = target;

        public override List<GameObject> GetSpawnPoints() =>
            _spawnPoints;
    }
}
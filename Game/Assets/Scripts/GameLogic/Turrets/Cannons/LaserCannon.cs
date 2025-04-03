using System.Collections.Generic;
using System.Linq;
using Data.Enums;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets.Projectiles;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.FactoryServices;
using UnityEngine;

namespace GameLogic.Turrets.Cannons
{
    public class LaserCannon : Cannon
    {
        [SerializeField] 
        private List<GameObject> _spawnPoints;
        [SerializeField]
        private List<ParticleSystem> _flashFx;
        [SerializeField]
        private AudioSource _audioSource;
        
        private ProjectilePool _projectilePool;
        private ProjectileType _projectileType;
        private TurretData _turretData;
        private Enemy _target;

        private float _reload;
        private float _timer;

        public override void Init(ProjectilePool projectilePool, TurretData turretData)
        {
            _projectilePool = projectilePool;
            _turretData = turretData;
            _projectileType = turretData.ProjectileType;

            _reload = _turretData.TurretPreferences.Reload;
            _timer = _reload;
            
            SetPitch(_audioSource);
        }

        private void Update()
        {
            if (_target == null || !_target.gameObject.activeSelf)
                foreach (var flash in _flashFx)
                {
                    flash.Stop();
                    _audioSource.Stop();
                }
            
            Fire();
        }

        protected override void Fire()
        {
            _timer -= Time.deltaTime;
            SpawnLaser();
        }

        public override void StopAttack()
        {
            _target = null;
        }

        private void SpawnLaser()
        {
            if (_timer <= 0 && _target != null && _target.gameObject.activeSelf)
            {
                foreach (var spawnPoint in _spawnPoints)
                {
                    var laser = _projectilePool.GetProjectile
                        (_projectileType, spawnPoint.transform.position);

                    if (laser == null)
                    {
                        _projectilePool.CreateProjectileForType(_projectileType);
                        laser =_projectilePool.GetProjectile(_projectileType, spawnPoint.transform.position);
                    }
                    
                    laser.SetDamage(_turretData.TurretPreferences.Damage);
                    laser.SetTarget(_target);
                    
                }

                foreach (var flash in _flashFx)
                    flash.Play();
                _audioSource.Play();

                _timer = _reload;
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
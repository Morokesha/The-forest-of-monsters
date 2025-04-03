using System;
using System.Collections.Generic;
using Data.TurretsData;
using GameLogic.Enemys;
using UnityEngine;

namespace GameLogic.Turrets.Projectiles
{
    public class PlasmaProjectile : Projectile
    {
        [SerializeField] 
        private List<GameObject> _visualList;
        [SerializeField] 
        private List<ParticleSystem> _explosionFx;
        [SerializeField]
        private LayerMask _targetLayer;
        
        private ProjectilePool.ProjectilePool _projectilePool;
        private ProjectileData _projectileData;
        private Enemy _target;
        
        private Vector3 _direction;
        private int _damage;
        private float _moveSpeed;

        private float _timer;
        private readonly float _timeToDestroy = 0.4f;
        private readonly float _radiusDestroy = 3f;
        private int _sphereCastCount;
        private bool _isDeletedTime;

        public override void Init(ProjectileData projectileData , ProjectilePool.ProjectilePool projectilePool)
        {
            _projectileData = projectileData;
            _projectilePool = projectilePool;
            
            _moveSpeed = _projectileData.MovementSpeed;
        }

        private void OnEnable()
        {
            foreach (var visual in _visualList)
                visual.SetActive(true);
            
            _isDeletedTime = false;
            _timer = _timeToDestroy;
        }

        public override void SetDamage(int damage) =>
            _damage = damage;
        
        private void Update()
        {
            if (_target != null && _target.gameObject.activeSelf && _sphereCastCount == 0)
                MoveToTarget();
            if (_target == null || !_target.gameObject.activeSelf)
            {
                transform.Translate(_direction * _projectileData.MovementSpeed);
                _sphereCastCount = 0;
            }
            
            DestroyProjectile();
        }

        private void MoveToTarget()
        {
            if (_target != null && _target.gameObject.activeSelf)
            {
                transform.position = Vector3.MoveTowards
                (transform.position, _target.transform.position + Vector3.up,
                    _projectileData.MovementSpeed * Time.deltaTime);
            }
            else
                _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
        }
        
        private void SphereCast()
        {
            _sphereCastCount += 1;

            if (_sphereCastCount == 1)
            {
                foreach (var visual in _visualList)
                    visual.SetActive(false);
                foreach (var fx in _explosionFx)
                    fx.Play();
            
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radiusDestroy, _targetLayer);
            
                foreach (Collider col in hitColliders)
                {
                    if (col.TryGetComponent(out Enemy enemy))
                        enemy.TakeDamage(_damage);
                }
                
                _isDeletedTime = true;
            }
        }

        public override void SetTarget(Enemy target)
        {
            _target = target;
            _direction = _target.transform.position - transform.position;
        }
        
        public override void DestroyProjectile()
        {
            if (_isDeletedTime == true)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    foreach (var fx in _explosionFx)
                        fx.Stop();
                    _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
                    _sphereCastCount = 0;
                    _isDeletedTime = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                SphereCast();
        }
    }
}
using System;
using System.Collections;
using Data;
using Data.TurretsData;
using DG.Tweening;
using GameLogic.Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic.Turrets.Projectiles
{
    public class RocketProjectile : Projectile
    {
        [SerializeField]
        private ParticleSystem _explosionFx;
        [SerializeField]
        private LayerMask _targetLayer;
        [SerializeField]
        private GameObject _visual;
        
        private ProjectileData _projectileData;
        private ProjectilePool.ProjectilePool _projectilePool;
        private Enemy _target;
        
        private Vector3 _direction;
        private int _damage;
        
        private float _rocketSpeed;
        private readonly float _radiusDestroy = 6f;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;
        private Vector3 _lookTargetPos;
        
        private readonly float _timeToDestroy = 0.3f;
        private float _timer;
        private int _sphereCastCount;
        
        private bool _isDeletedTime;

        public override void Init(ProjectileData projectileData , ProjectilePool.ProjectilePool projectilePool)
        {
            _projectileData = projectileData;
            _projectilePool = projectilePool;

            _rocketSpeed = _projectileData.MovementSpeed;
        }
        void Start()
        {
            _initialPosition = transform.position;
            _initialPosition.y = 1.5f;
        }

        private void OnEnable() => 
            _visual.SetActive(true);

        private void Update()
        {
            if (_target != null && _target.gameObject.activeSelf)
                SetForward();
            
            DestroyProjectile();
        }
        
        private void Shot()
        {
            float timeToReachTarget = Vector3.Distance(_initialPosition, _targetPosition) / _rocketSpeed;
            transform.DOJump(_targetPosition, 4, 1, timeToReachTarget).OnComplete(SphereCast);

            _lookTargetPos = _targetPosition;
        }


        public override void SetDamage(int damage)
        {
            _damage = damage;
        }

        public override void SetTarget(Enemy target)
        {
            _target = target;

            if (_target != null) 
                _targetPosition = _target.transform.position;

            SetForward();

            Shot();
        }

        private void SetForward()
        {
            _direction = (_targetPosition - transform.position).normalized;
            _visual.transform.forward = _direction;
        }

        public override void DestroyProjectile()
        {
            if (_isDeletedTime == true)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                { 
                    _explosionFx.Stop();
                    _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
                    _sphereCastCount = 0;
                    _isDeletedTime = false;
                }
            }
        }

        private void SphereCast()
        {
            _sphereCastCount += 1;

            if (_sphereCastCount == 1)
            {
                _visual.SetActive(false);
                _explosionFx.Play();
            
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radiusDestroy, _targetLayer);
            
                foreach (Collider col in hitColliders)
                {
                    if (col.TryGetComponent(out Enemy enemy))
                        enemy.TakeDamage(_damage);
                }

                _isDeletedTime = true;
                _timer = _timeToDestroy;
            }
            
        }

        private void OnDestroy() => 
            transform.DOKill();
    }
}
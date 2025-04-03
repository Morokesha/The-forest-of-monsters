using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.TurretsData;
using GameLogic.Enemys;
using UnityEngine;

namespace GameLogic.Turrets.Projectiles
{
    public class LaserProjectile : Projectile
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        
        private ProjectilePool.ProjectilePool _projectilePool;
        private ProjectileData _projectileData;
        private Enemy _target;
        
        private int _damage;

        private readonly float _timeToDestroy = 5f;
        private float _timer;

        private Vector3 _direction;

        public override void Init(ProjectileData projectileData , ProjectilePool.ProjectilePool projectilePool)
        {
            _projectileData = projectileData;
            _projectilePool = projectilePool;
        }

        private void OnEnable()
        {
            _timer = _timeToDestroy;
        }

        public override void SetDamage(int damage)
        {
            _damage = damage;
        }
        
        private void Update()
        {
            MoveToTarget();
            DestroyProjectile();
        }

        private void MoveToTarget()
        {
            if (_target != null && _target.gameObject.activeSelf)
            {
                transform.position = Vector3.MoveTowards
                (transform.position, _target.transform.position,
                    _projectileData.MovementSpeed * Time.deltaTime);
                transform.forward = _target.transform.position - transform.position;
            }
            else
                _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
        }

        public override void SetTarget(Enemy target)
        {
            _target = target;
            _direction = (_target.transform.position - transform.position).normalized;
            transform.forward = _direction;

            _meshRenderer.enabled = true;
        }
        
        public override void DestroyProjectile()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0) 
                _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
            }
        }
    }
}
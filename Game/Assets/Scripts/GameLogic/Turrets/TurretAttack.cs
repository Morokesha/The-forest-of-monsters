using System;
using Data;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets.Cannons;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.FactoryServices;
using UnityEngine;

namespace GameLogic.Turrets
{
    public class TurretAttack : MonoBehaviour
    {
        [SerializeField] 
        private LayerMask _targetLayer;
        
        private ProjectilePool _projectilePool;
        private TurretData _turretData;
        private TurretRotation _turretRotation;
        private Enemy _target;
        private Cannon _cannon;

        private float _radiusAttack;

        private bool _gameOver;

        public void Init(ProjectilePool projectilePool, TurretData turretData, TurretRotation turretRotation,
            Cannon cannon)
        {
            _projectilePool = projectilePool;
            _turretData = turretData;
            _turretRotation = turretRotation;
            _cannon = cannon;
            
            _cannon.Init(_projectilePool, _turretData);

            _radiusAttack = _turretData.TurretPreferences.AttackRadius;
            _gameOver = false;
        }

        private void Update()
        {
            if (_gameOver == false)
            {
                if (_target == null || !_target.gameObject.activeSelf)
                {
                    FindTarget();
                    _turretRotation.LookToLastPos();
                }
                else
                {
                    if (!TargetOutsideRadiusAttack())
                    {
                        _turretRotation.RotateTurret(_target);
                    
                        if (_turretRotation.LookToTarget(_target)) 
                            _cannon.SetTarget(_target);
                    }
                    else
                    {
                        _target = null;
                        _cannon.StopAttack();
                    }
                }
            }
        }

        public void StopAttack()
        {
            _gameOver = true;
            _cannon.StopAttack();
            _target = null;
        }

        private bool TargetOutsideRadiusAttack()
        {
            if (_target != null && _target.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, _target.transform.position);
                if (distance > _radiusAttack)
                    return true;

                return false;
            }
            return false;
        }

        private void FindTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radiusAttack, _targetLayer);

            float closestDistance = Mathf.Infinity;
            
            foreach (Collider col in hitColliders)
            {
                if (col.TryGetComponent(out Enemy enemy))
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        _target = enemy;
                    }
                }
            }
        }
    }
}
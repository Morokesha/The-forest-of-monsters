using Data;
using Data.TurretsData;
using GameLogic.Enemys;
using UnityEngine;

namespace GameLogic.Turrets.Projectiles
{
    public class BulletProjectile : Projectile
    {
        private ProjectileData _projectileData;
        private ProjectilePool.ProjectilePool _projectilePool;
        private Enemy _target;
        
        private int _damage;

        private readonly float _timeToDestroy = 5f;
        private float _timer;

        public override void Init(ProjectileData projectileData , ProjectilePool.ProjectilePool projectilePool)
        {
            _projectileData = projectileData;
            _projectilePool = projectilePool;
        }

        public override void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void OnEnable()
        {
            _timer = _timeToDestroy;
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
            }
            else
                _projectilePool.ReturnProjectile(_projectileData.ProjectileType, this);
        }

    public override void SetTarget(Enemy target)
        {
            _target = target;
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
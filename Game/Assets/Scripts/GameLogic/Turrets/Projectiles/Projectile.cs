using Data.TurretsData;
using GameLogic.Enemys;
using UnityEngine;

namespace GameLogic.Turrets.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract void Init(ProjectileData projectileData , ProjectilePool.ProjectilePool projectilePool);
        public abstract void SetDamage(int damage);
        public abstract void SetTarget(Enemy target);

        public abstract void DestroyProjectile();
    }
}
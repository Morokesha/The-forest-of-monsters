using System.Collections.Generic;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using UnityEngine;

namespace GameLogic.Turrets.Cannons
{
    public abstract class Cannon : MonoBehaviour
    {
        protected virtual void SetPitch(AudioSource audioSource) => 
            audioSource.pitch = Random.Range(0.88f, 1.12f);
        
        public abstract void Init(ProjectilePool projectilePool, TurretData turretData);
        protected abstract void Fire();

        public abstract void StopAttack();
        public abstract void SetTarget(Enemy target);
        public abstract List<GameObject> GetSpawnPoints();
    }
}
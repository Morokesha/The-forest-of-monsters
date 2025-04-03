using Data;
using Data.Enums;
using Data.LevelsData;
using Data.TurretsData;
using GameLogic;
using GameLogic.Enemys;
using GameLogic.Turrets;
using GameLogic.Turrets.Projectiles;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.ResourceRepositoryService;
using Services.SoundServices;
using UnityEngine;

namespace Services.FactoryServices
{
    public interface IFactoryService
    {
        public void CreateLevel(LevelData levelData);

        public Turret CreateTurret(Transform parent, Transform transform, TurretData turretData,
            ProjectilePool projectilePool);

        public Enemy CreateEnemy(Vector3 spawnPoint, EnemyType enemyType,
            ResourceRepository resourceRepository, EnemySpawner enemySpawner);
        public MainBuilding CreateMainBuilding(Transform transform);
        public Projectile CreateProjectile(ProjectileType type,ProjectilePool projectilePool , Vector3 position);
        public SoundService CreateSoundService();
    }
}
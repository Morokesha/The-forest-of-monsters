using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Enums;
using Data.LevelsData;
using Data.TurretsData;
using GameLogic;
using GameLogic.Enemys;
using GameLogic.Turrets;
using GameLogic.Turrets.Projectiles;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.AssetServices;
using Services.ResourceRepositoryService;
using Services.SaveLoadServices;
using Services.SoundServices;
using Services.StaticDataServices;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Services.FactoryServices
{
    public class FactoryService : IFactoryService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticData _staticData;
        private MainBuilding _mainBuilding;
        private SoundService _soundService;

        public FactoryService( IAssetProvider assetProvider, IStaticData staticData)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
        }

        public void CreateLevel(LevelData levelData) => 
            _assetProvider.Instantiate<GameObject>(levelData.LevelPf);

        public Turret CreateTurret(Transform parent, Transform transform ,TurretData turretData, 
            ProjectilePool projectilePool)
        {
            Turret turret = _assetProvider.Instantiate<Turret>(turretData.TurretPf, parent);
            turret.transform.position = transform.position;
            turret.Init(projectilePool, turretData, _soundService);

            return turret;
        }

        public Enemy CreateEnemy(Vector3  spawnPoint, EnemyType enemyType,
            ResourceRepository resourceRepository, EnemySpawner enemySpawner)
        {
            EnemyData enemyData = _staticData.GetEnemyData(enemyType);
            Enemy enemy = _assetProvider.Instantiate<Enemy>(enemyData.EnemyPf, spawnPoint);
            enemy.Init(resourceRepository, enemyData, enemySpawner, _mainBuilding);

            return enemy;
        }

        public MainBuilding CreateMainBuilding(Transform transform)
        {
            _mainBuilding =
                _assetProvider.Instantiate<MainBuilding>(AssetPath.MainBuildingPath, transform.position);
            _mainBuilding.transform.rotation = transform.rotation;

            return _mainBuilding;
        }

        public Projectile CreateProjectile(ProjectileType type,ProjectilePool projectilePool , Vector3 position)
        {
            ProjectileData projectileData = _staticData.GetProjectileData(type);
            Projectile projectile = _assetProvider.Instantiate<Projectile>(projectileData.ProjectileTemplate, position);
            projectile.Init(projectileData, projectilePool);

            return projectile;
        }

        public SoundService CreateSoundService()
        {
            _soundService = _assetProvider.Instantiate<SoundService>(AssetPath.SoundServicePath);
            return _soundService;
        }
    }
}
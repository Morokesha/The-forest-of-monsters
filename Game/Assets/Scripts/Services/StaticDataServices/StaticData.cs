using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Enums;
using Data.LevelsData;
using Data.TurretsData;
using GameLogic.Turrets;
using Services.AssetServices;

namespace Services.StaticDataServices
{
    public class StaticData : IStaticData
    {
        private readonly IAssetProvider _assetProvider;
        
        private Turret _turretTemplate;
        private List<TurretData> _turretDataList;
        private List<EnemyData> _enemyDataList;
        private List<ProjectileData> _projectileDataList;
        private List<LevelData> _levelsData;
        public StaticData(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void Init() => 
            Load();

        private void Load()
        {
            _levelsData = new List<LevelData>();
            _levelsData = _assetProvider.LoadAll<LevelData>(AssetPath.LevelDataPath).ToList();
            _turretDataList = new List<TurretData>();
            _turretDataList = _assetProvider.LoadAll<TurretData>(AssetPath.TurretDataPath).ToList();
            _enemyDataList = new List<EnemyData>();
            _enemyDataList = _assetProvider.LoadAll<EnemyData>(AssetPath.EnemyDataPath).ToList();
            _projectileDataList = new List<ProjectileData>();
            _projectileDataList = _assetProvider.LoadAll<ProjectileData>(AssetPath.ProjectileDataPath).ToList();
        }

        public TurretData GetTurretData(TurretType turretType, UpgradeStage nextUpgradeStage) =>
            _turretDataList.FirstOrDefault(turretData => turretType == turretData.TurretType && 
                                                         nextUpgradeStage == turretData.UpgradeStage);

        public List<TurretData> GetDefaultTurretList() =>
            _turretDataList.Where(turretData => turretData.UpgradeStage == UpgradeStage.Stage1).
                OrderBy(x => x.Price).ToList();

        public EnemyData GetEnemyData(EnemyType enemyType) => 
            _enemyDataList.FirstOrDefault(enemy => enemy.EnemyType == enemyType);

        public ProjectileData GetProjectileData(ProjectileType type) =>
            _projectileDataList.FirstOrDefault(data => data.ProjectileType == type);

        public LevelData GetLevelData(int index) =>
            _levelsData.FirstOrDefault(levelData => levelData.Level == index);
    }
}
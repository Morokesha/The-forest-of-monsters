using System.Collections.Generic;
using System.Net.Sockets;
using Data;
using Data.Enums;
using Data.LevelsData;
using Data.TurretsData;
using GameLogic.Enemys;
using GameLogic.Turrets;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticData
    {
        void Init();
        public TurretData GetTurretData(TurretType turretType, UpgradeStage nextUpgradeStage);
        public List<TurretData> GetDefaultTurretList();
        public EnemyData GetEnemyData(EnemyType enemyType);
        ProjectileData GetProjectileData(ProjectileType type);
        LevelData GetLevelData(int index);
    }
}
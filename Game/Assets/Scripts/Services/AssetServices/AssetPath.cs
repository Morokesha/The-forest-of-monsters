using UnityEngine;

namespace Services.AssetServices
{
    public static class AssetPath
    {
        public const string LevelDataPath = "Data/LevelData";
        public const string TurretDataPath = "Data/TurretData/";
        public const string ProjectileDataPath = "Data/TurretData/ProjectileData/";
        public const string EnemyDataPath = "Data/EnemyData/";
        public const string MainBuildingPath = "Level Template/MainBuilding";
        public const string SoundServicePath = "Level Template/AudioService";

        [Header("UI")]
        public const string UIRootPath = "UI/UIRoot";
        public const string BuyTurretMenuPath = "UI/BuyTurretMenu";
        public const string UpgradeTurretMenuPath = "UI/UpgradeTurretMenu";
        public const string SliderToEndLevelPath = "UI/SliderToEndLevel";
        public const string EndGameWindowPath = "UI/EndGameWindow";
        public const string GoldCounterPath = "UI/GoldCounterContainer";
        public const string SoundSettingsMenuPath = "UI/SoundSettingsPanel";
        public const string SettingsMenuPath = "UI/SettingsPanel";
        public const string SelectLevelBtn = "UI/SelectLevelBtn";
        public const string TutorialInfoPath = "UI/TutorialInfo";
    }
}
using Data.Enums;
using GameLogic.Turrets;
using UnityEngine;

namespace Data.TurretsData
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/TurretData", order = 0)]
    public class TurretData : ScriptableObject
    {
        public string TurretName;
        public string TurretNameEn;
        
        [Space(2)]
        
        public Turret TurretPf;
        public TurretType TurretType;
        public UpgradeStage UpgradeStage;
        public UpgradeStage NextUpgradeStage;
        public ProjectileType ProjectileType;

        [Space(2)] 
        public TurretPreferences TurretPreferences;

        public int Price;

        [TextArea] 
        public string Description;
        [TextArea] 
        public string DescriptionEn;

    }
}
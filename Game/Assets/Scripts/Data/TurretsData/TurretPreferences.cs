using System;
using UnityEngine;

namespace Data.TurretsData
{
    [Serializable]
    public class TurretPreferences
    {
        public Sprite SpriteTurret;
        
        [Space(4)]
        public int Damage;
        public float AttackRadius;
        public float Reload;
    }
}
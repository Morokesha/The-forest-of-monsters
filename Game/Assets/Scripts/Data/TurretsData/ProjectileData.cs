using Data.Enums;
using GameLogic.Turrets.Projectiles;
using UnityEngine;

namespace Data.TurretsData
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/ProjectileData", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        public ProjectileType ProjectileType;
        public Projectile ProjectileTemplate;
        public float MovementSpeed;
    }
}
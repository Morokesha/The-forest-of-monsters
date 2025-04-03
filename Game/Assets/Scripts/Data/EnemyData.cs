using Data.Enums;
using GameLogic.Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public EnemyType EnemyType;

        public Enemy EnemyPf;
        
        public int Damage;
        public float MoveSpeed;
        public HealthData HealthData;
        public int EnemyLootGold;
    }
}
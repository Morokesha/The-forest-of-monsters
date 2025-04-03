using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.LevelsData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public int Level;
        public GameObject LevelPf;

        public List<WavePreferences> WavePreferences;
        public int StartGoldOnLevel;
    }
}
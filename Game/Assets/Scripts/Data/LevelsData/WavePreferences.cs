using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Data.LevelsData
{
    [Serializable]
    public class WavePreferences
    {
        [FormerlySerializedAs("_LevelPartPreferencesList")] public List<LevelPartPreferences> LevelPartPreferencesList;
    }
}
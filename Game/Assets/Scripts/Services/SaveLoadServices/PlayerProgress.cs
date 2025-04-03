using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.SaveLoadServices
{
    [Serializable]
    public class PlayerProgress
    {
        public bool SoundActivated = true;
        public bool MusicActivated = true;

        public int CurrentLevel = 1;
        public int OpenLevels = 1;
        public int LevelsCount = 8;

        public bool TutorialIsDone = false;

        public void SaveCurrentLevel(int index)
        {
                CurrentLevel = index;
                
            if (index <= LevelsCount && index > OpenLevels)
                OpenLevels = CurrentLevel;
        }

        public void ChangeMusicPreferences(bool active)
        {
            MusicActivated = active;
        }

        public void ChangeSoundPreferences(bool active)
        {
            SoundActivated = active;
        }

        public void CompleteTutorial()
        {
            TutorialIsDone = true;
        }
    }
}
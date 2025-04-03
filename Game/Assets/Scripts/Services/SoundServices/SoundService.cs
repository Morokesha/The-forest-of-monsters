using System;
using System.Collections.Generic;
using System.Linq;
using Services.SaveLoadServices;
using UI.MainMenuElements;
using UnityEngine;

namespace Services.SoundServices
{
    public class SoundService : MonoBehaviour
    {
        [Header("Sound Volume")] 
        [SerializeField] [Range(0, 1f)]
        private float _soundVolume = 0.35f;
        [SerializeField] [Range(0, 1f)]
        private float _musicVolume = 0.4f;
        
        [Space(2)]
        [Header("Sound Sources")]
        [SerializeField]
        private AudioSource _backgroundSource;
        [SerializeField] 
        private AudioSource _soundsSource;
        
        [Space(3)]
        [Header("Audio Clips")]
        [SerializeField] 
        private AudioClip _looseSfx;
        [SerializeField]
        private AudioClip _winnerSfx;
        [SerializeField]
        private AudioClip _clickSfx;
        [SerializeField] 
        private AudioClip _buildingPlacementSfx;
        [SerializeField]
        private AudioClip _buySoundSfx;
        [SerializeField]
        private AudioClip _towerLvlUpSfx;
        [SerializeField]
        private AudioClip _sellTowerSfx;
        
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;
        private SoundSettingsMenu _soundSettingMenu;

        private bool _soundActivated;
        private bool _musicActivated;
        
        private List<SoundSource> _soundSources;

        public void Init(ISaveLoadService saveLoadService, SoundSettingsMenu soundSettingsMenu)
        {
            _saveLoadService = saveLoadService;
            _progress = _saveLoadService.GetProgress();
            _soundSettingMenu = soundSettingsMenu;
            
            LoadData();
            
            _soundSettingMenu.OnMusicAction += ActivateMusic;
            _soundSettingMenu.OnSoundAction += ActivateSound;
        }

        private void Start() => 
            SetLoadSettings();

        public void PlayGameOverSfx()
        {
            _backgroundSource.loop = false;
            _backgroundSource.clip = _looseSfx;
            _backgroundSource.PlayOneShot(_looseSfx);
        }

        public void PlayWinnerSfx()
        {
            _backgroundSource.loop = false;
            _backgroundSource.clip = _winnerSfx;
            _backgroundSource.PlayOneShot(_winnerSfx);
        }

        public void PlayClickSfx() => 
            _soundsSource.PlayOneShot(_clickSfx);
        
        public void PlayTurretPlacementSfx() =>
            _soundsSource.PlayOneShot(_buildingPlacementSfx);

        public void PlayBuySfx() => 
            _soundsSource.PlayOneShot(_buySoundSfx);

        public void PlayTowerLvlUpSfx() =>
            _soundsSource.PlayOneShot(_towerLvlUpSfx);
        
        public void PlaySellTowerSfx() => 
            _soundsSource.PlayOneShot(_sellTowerSfx);

        public bool GetSoundValue() =>
            _soundActivated;

        public void StopMusic()
        {
            _backgroundSource.volume = 0f;
            
            foreach (var soundSource in _soundSources)
                soundSource.GetAudioSource().volume = 0f;
        }

        public void PlayMusic()
        {
            SetLoadSettings();
        }

        private void LoadData()
        {
            _soundSources = new List<SoundSource>();
            _backgroundSource = GetComponent<AudioSource>();
            
            _soundActivated = _progress.SoundActivated;
            _musicActivated = _progress.MusicActivated;
        }
        private void SetLoadSettings()
        {
            FindSoundSourcesAndSetValue();

            _backgroundSource.volume = _musicActivated ? _musicVolume : 0f;
            _soundsSource.volume = _soundActivated ? _soundVolume : 0f;
            
            _soundSettingMenu.SetSliderPositionOnLoad(_soundActivated, _musicActivated);
        }

        private void FindSoundSourcesAndSetValue()
        {
            _soundSources.Clear();
            _soundSources = FindObjectsOfType<SoundSource>().ToList();

            foreach (var soundSource in _soundSources)
                soundSource.GetAudioSource().volume = _soundActivated ? _soundVolume : 0f;
        }


        private void ActivateMusic(bool active)
        {
            _musicActivated = active;
            
            _backgroundSource.volume = _musicActivated ? _musicVolume : 0f;
            
            _progress.ChangeMusicPreferences(_musicActivated);
            _saveLoadService.Save();
        }

        private void ActivateSound(bool active)
        {
            _soundActivated = active;
            FindSoundSourcesAndSetValue();
            
            _soundsSource.volume = _soundActivated ? _soundVolume : 0f;
            
            _progress.ChangeSoundPreferences(_soundActivated);
            _saveLoadService.Save();
        }

        private void OnDestroy()
        {
            _soundSettingMenu.OnMusicAction -= ActivateMusic;
            _soundSettingMenu.OnSoundAction -= ActivateSound;
        }

    }
}
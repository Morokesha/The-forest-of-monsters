using Data.TurretsData;
using GameLogic.Turrets.Cannons;
using GameLogic.Turrets.Projectiles.ProjectilePool;
using Services.FactoryServices;
using Services.SoundServices;
using UnityEngine;

namespace GameLogic.Turrets
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _sphereRadiusAttack;
        [SerializeField]
        private TurretRotation _turretRotation;
        [SerializeField]
        private TurretAttack _turretAttack;
        [SerializeField]
        private Cannon _cannon;
        [SerializeField]
        private AudioSource _audioSource;
        
        private TurretData _turretData;
        private ProjectilePool _projectilePool;
        private SoundService _soundService;

        public void Init(ProjectilePool projectilePool, TurretData turretData, SoundService soundService)
        {
            _projectilePool = projectilePool; 
            _turretData = turretData;
            _soundService = soundService;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            
            _turretAttack.Init(_projectilePool, _turretData, _turretRotation, _cannon);
            
            SetRadiusSphereAttack();

            SoundSourceSetValue();
            _soundService.PlayTurretPlacementSfx();
        }

        public TurretData GetTurretData() =>
            _turretData;

        public void StopGame()
        {
            _turretAttack.StopAttack();
        }
        public void ShowRadius() =>
            _sphereRadiusAttack.gameObject.SetActive(true);
        public void HideRadius() =>
            _sphereRadiusAttack.gameObject.SetActive(false);

        private void SetRadiusSphereAttack()
        {
            _sphereRadiusAttack.transform.localScale = new Vector3
                (_turretData.TurretPreferences.AttackRadius * 2, 0.1f, 
                    _turretData.TurretPreferences.AttackRadius * 2);
            _sphereRadiusAttack.gameObject.SetActive(false);
        }

        private void SoundSourceSetValue()
        {
            if (_soundService.GetSoundValue() == false) 
                _audioSource.volume = 0;
        }

    }
}
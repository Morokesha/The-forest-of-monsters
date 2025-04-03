using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Enums;
using Services.FactoryServices;
using Services.ResourceRepositoryService;
using Services.SaveLoadServices;
using Services.StaticDataServices;
using UnityEngine;

namespace GameLogic.Enemys
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action<float> EnemyAmountChanged;
        public event Action LevelCompleted;
        
        
        [SerializeField] private List<GameObject> _spawnPoints;
        [SerializeField] private EnemyPool _enemyPool;
        
        private IFactoryService _factoryService;
        private IStaticData _staticData;
        private PlayerProgress _playerProgress;
        private ResourceRepository _resourceRepository;
        private Transform _setDestinationPoint;

        private readonly float _timeToSpawnNewEnemy = 1.2f;
        private readonly float _timeToSpawnNewWave = 8f;
        private WaitForSeconds _waitToSpawn;
        private WaitForSeconds _waitNewWave;
        
        private int _currentWave;
        private int _enemyAmount;
        private float _currentDeathEnemy;

        public void Init(IFactoryService factoryService, IStaticData staticData, PlayerProgress playerProgress, 
            ResourceRepository resourceRepository, Transform setDestinationPoint)
        {
            _factoryService = factoryService;
            _staticData = staticData;
            _playerProgress = playerProgress;
            _resourceRepository = resourceRepository;
            _setDestinationPoint = setDestinationPoint;
            
            _enemyPool.Init(_factoryService, _spawnPoints, _resourceRepository, this);
            
            SetEnemyAmount();
            
            print(_enemyAmount + " Enemy Amount");
        }

        private void Start()
        {
            _waitToSpawn = new WaitForSeconds(_timeToSpawnNewEnemy);
            _waitNewWave = new WaitForSeconds(_timeToSpawnNewWave);
            
            StartCoroutine(SpawnEnemyCo());
        }

        public int GetEnemyAmount() =>
            _enemyAmount;
        
        public void StopSpawn() => 
            StopAllCoroutines();

        public void UpdateCurrentDeathEnemy()
        {
            _currentDeathEnemy += 1;
            EnemyAmountChanged?.Invoke(_currentDeathEnemy);
            
            const float tolerance = 0;
            
            if ((_currentDeathEnemy - _enemyAmount) >= tolerance) 
                LevelCompleted?.Invoke();
        }

        private void SetEnemyAmount()
        {
            foreach (var wavePreference in _staticData.GetLevelData(_playerProgress.CurrentLevel).
                         WavePreferences)
            {
                foreach (var levelPart in wavePreference.LevelPartPreferencesList)
                {
                    _enemyAmount += levelPart.CrabCount;
                    _enemyAmount += levelPart.BatCount;
                    _enemyAmount += levelPart.SceletonCount;
                    _enemyAmount += levelPart.OrcCount;
                    _enemyAmount += levelPart.BlackKnightCount;
                    _enemyAmount += levelPart.MageCount;
                }
            }
        }
        

        private IEnumerator SpawnEnemyCo()
        {
            yield return _waitNewWave;
            
            
            for (var index = 0; index < _staticData.GetLevelData(_playerProgress.CurrentLevel).
                     WavePreferences.Count; index++)
            {
                if (_currentWave < _staticData.GetLevelData(_playerProgress.CurrentLevel).WavePreferences.Count)
                {
                    var wavePrefs = _staticData.GetLevelData(_playerProgress.CurrentLevel).
                        WavePreferences[_currentWave];
                
                    foreach (var partWave in wavePrefs.LevelPartPreferencesList)
                    {
                        for (int i = 0; i < _spawnPoints.Count; i++)
                        {
                            Vector3 position = _spawnPoints[i].transform.position;
                            
                            yield return SpawnEnemyForType(GetEnemySpawnAmount(partWave.CrabCount),
                                position, EnemyType.Crab);
                            yield return SpawnEnemyForType(GetEnemySpawnAmount(partWave.SceletonCount), 
                                position, EnemyType.Skeleton);
                            yield return SpawnEnemyForType(GetEnemySpawnAmount(partWave.OrcCount),
                                position, EnemyType.Ork);
                            yield return SpawnEnemyForType(GetEnemySpawnAmount(partWave.BatCount),
                                position, EnemyType.Bat);
                            yield return SpawnEnemyForType
                            (GetEnemySpawnAmount(partWave.BlackKnightCount),
                                position, EnemyType.BlackKnight);
                            yield return SpawnEnemyForType(GetEnemySpawnAmount(partWave.MageCount), 
                                position, EnemyType.EvilMage);
                        }
                    }
                    
                    _currentWave += 1;
                yield return _waitNewWave;
                }
            }
            
            yield return _waitNewWave;
        }

        private int GetEnemySpawnAmount(int amount)
        {
            float value = _spawnPoints.Count;

            return (int)(amount / value);
        }

        private IEnumerator SpawnEnemyForType(int enemyAmount, Vector3 spawnPoint, EnemyType type)
        {
            while (enemyAmount > 0)
            { 
                Enemy enemy = _enemyPool.GetEnemy(type, spawnPoint);

                if (enemy == null)
                {
                    _enemyPool.CreateNewEnemy(type);
                    enemy =  _enemyPool.GetEnemy(type, spawnPoint);
                }
                
                enemy.MoveToTarget(_setDestinationPoint.position);
                
                enemy.OnDead += EnemyOnDead;
                
                enemyAmount -= 1;
                yield return _waitToSpawn;
            }
        }

        private void EnemyOnDead(EnemyType type,Enemy enemy)
        {
            print("Return " + _currentDeathEnemy);
            _enemyPool.ReturnEnemy(type,enemy);
            enemy.OnDead -= EnemyOnDead;
        }
    }
}
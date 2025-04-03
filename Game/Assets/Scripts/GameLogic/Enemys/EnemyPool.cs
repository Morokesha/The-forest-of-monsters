using Data.Enums;
using Services.FactoryServices;
using Services.ResourceRepositoryService;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.Enemys
{ 

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPoolEntry
    {
        public EnemyType enemyType; // Ключ - тип врага
        public int poolSize; // Размер пула для этого типа
    }

    public List<EnemyPoolEntry> enemyPoolEntries; // Список настроек пула

    private Dictionary<EnemyType, Queue<Enemy>> _enemyPool; // Сам пул
    private IFactoryService _factoryService;
    private List<GameObject> _spawnPoint;
    private ResourceRepository _resourceRepository;
    private EnemySpawner _enemySpawner;

    public void Init(IFactoryService factoryService, List<GameObject> spawnPoint, ResourceRepository resourceRepository,
        EnemySpawner enemySpawner)
    {
        _enemyPool = new Dictionary<EnemyType, Queue<Enemy>>();
        _factoryService = factoryService;
        _spawnPoint = spawnPoint;
        _resourceRepository = resourceRepository;
        _enemySpawner = enemySpawner;

        // Инициализируем пул для каждого типа врага
        foreach (EnemyPoolEntry entry in enemyPoolEntries)
        {
            Queue<Enemy> objectPool = new Queue<Enemy>();

            // Создаем экземпляры врагов и добавляем их в пул
            for (int i = 0; i < entry.poolSize; i++)
            {
                foreach (var enemy in _spawnPoint.Select
                         (t => factoryService.CreateEnemy(t.transform.position, entry.enemyType, 
                             resourceRepository, enemySpawner)))
                {
                    enemy.gameObject.transform.parent = transform;
                    enemy.gameObject.SetActive(false); // Деактивируем объект, пока он в пуле
                    objectPool.Enqueue(enemy);
                }
            }

            _enemyPool.Add(entry.enemyType, objectPool);
        }
    }

    public void CreateNewEnemy(EnemyType enemyType)
    {
        int newEnemyAmount = 10;
                
        for (int i = 0; i < newEnemyAmount; i++)
        {
            foreach (var newEnemy in _spawnPoint.Select(t => _factoryService.CreateEnemy
                         (t.transform.position, enemyType,_resourceRepository, _enemySpawner)))
            {
                newEnemy.gameObject.transform.parent = transform;
                newEnemy.gameObject.SetActive(false);
                _enemyPool[enemyType].Enqueue(newEnemy);
            }
        }
    }

    // Метод для получения врага из пула
    public Enemy GetEnemy(EnemyType enemyType, Vector3 position)
    {
        if (_enemyPool.ContainsKey(enemyType))
        {
            if (_enemyPool[enemyType].Count > 0)
            {
                var enemy = _enemyPool[enemyType].Dequeue();
                enemy.SetPosition(position);
                enemy.gameObject.SetActive(true);
                return enemy;
            }

            Debug.Log("Пул для врага типа " + enemyType + " пуст!");
        }
        else
            Debug.Log("Враг типа " + enemyType + " не найден в пуле!");

        return null;
    }

    // Метод для возврата врага в пул
    public void ReturnEnemy(EnemyType enemyType, Enemy enemy)
    {
        if (_enemyPool.ContainsKey(enemyType))
        {
            enemy.gameObject.SetActive(false); // Деактивируем объект перед возвратом в пул
            _enemyPool[enemyType].Enqueue(enemy);
        }
        else
        {
            Debug.Log("Враг типа " + enemyType + " не найден в пуле!");
        }
        
        Debug.Log("RETURN TO POOL");
    }
}

}
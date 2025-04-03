using System.Collections.Generic;
using Data.Enums;
using Services.FactoryServices;
using UnityEngine;

namespace GameLogic.Turrets.Projectiles.ProjectilePool
{
    public class ProjectilePool : MonoBehaviour
    {
        [System.Serializable]
        public class ProjectilePoolEntry
        {
            public ProjectileType ProjectileType; // Ключ - тип врага
            public int poolSize; // Размер пула для этого типа
        }
        
        public List<ProjectilePoolEntry> projectilePoolEntries; // Список настроек пула
        
        private Dictionary<ProjectileType, Queue<Projectile>> _projectilePool; // Сам пул
        
        private IFactoryService _factoryService;
        private Vector3 _spawnPoint;

        public void Init(IFactoryService factoryService, Vector3 spawnPoint)
        {
            _projectilePool = new Dictionary<ProjectileType, Queue<Projectile>>();

            _factoryService = factoryService;
            _spawnPoint = spawnPoint;

            FillingPool();
        }

        private void FillingPool()
        {
            foreach (ProjectilePoolEntry entry in projectilePoolEntries)
            {
                Queue<Projectile> objectPool = new Queue<Projectile>();
                
                for (int i = 0; i < entry.poolSize; i++)
                {
                    Projectile projectile = _factoryService.CreateProjectile(entry.ProjectileType, this, 
                        _spawnPoint);
                    projectile.gameObject.transform.parent = transform;
                    projectile.gameObject.SetActive(false); // Деактивируем объект, пока он в пуле
                    objectPool.Enqueue(projectile);
                }
        
                _projectilePool.Add(entry.ProjectileType, objectPool);
            }
        }

        public void CreateProjectileForType(ProjectileType projectileType)
        {
            int newProjectileAmount = 10;
                
            for (int i = 0; i < newProjectileAmount; i++)
            {
                Projectile projectile = _factoryService.CreateProjectile(projectileType, this, 
                    _spawnPoint);
                projectile.gameObject.transform.parent = transform;
                projectile.gameObject.SetActive(false);
                _projectilePool[projectileType].Enqueue(projectile);
            }
        }

        public Projectile GetProjectile(ProjectileType projectileType, Vector3 spawnPoint)
        {
            if (_projectilePool.ContainsKey(projectileType))
            {
                Projectile projectile;
                if (_projectilePool[projectileType].Count > 0)
                {
                    projectile = _projectilePool[projectileType].Dequeue();
                    projectile.gameObject.transform.position = spawnPoint;
                    projectile.gameObject.SetActive(true); // Активируем объект при получении из пула
                    return projectile;
                }
        
                Debug.Log("Пул для снаряда типа " + projectileType + " пуст!");
            }
            else
                Debug.Log("Снаряд типа " + projectileType + " не найден в пуле!");
        
            return null;
        }
        
        public void ReturnProjectile(ProjectileType projectileType, Projectile projectile)
        {
            if (_projectilePool.ContainsKey(projectileType))
            {
                projectile.gameObject.SetActive(false); // Деактивируем объект перед возвратом в пул
                _projectilePool[projectileType].Enqueue(projectile);
            }
            else
                Debug.Log("Снаряд типа " + projectileType + " не найден в пуле!");
        }
    }
}
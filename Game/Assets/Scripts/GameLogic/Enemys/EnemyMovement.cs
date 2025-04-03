using System;
using Data;
using Services.StaticDataServices;
using UnityEngine;
using UnityEngine.AI;

namespace GameLogic.Enemys
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        
        private NavMeshAgent _agent;
        
        private EnemyData _enemyData;
        private MainBuilding _mainBuilding;
        private EnemySpawner _enemySpawner;
        
        private readonly string _moveId = "Move";
        private Vector3 _targetMove;

        public void Init(EnemyData enemyData, MainBuilding mainBuilding, EnemySpawner enemySpawner,
            NavMeshAgent agent)
        {
            _enemyData = enemyData;
            _mainBuilding = mainBuilding;
            _enemySpawner = enemySpawner;

            _agent = agent;
            _agent.speed = _enemyData.MoveSpeed;
            
            _mainBuilding.OnDestroyBuilding += OnDestroyBuilding;
        }
        

        public void SetTargetMove(Vector3 targetMove)
        {
            _targetMove = targetMove;
            
            Move();
        }

        private void OnDestroyBuilding()
        {
            if (_agent.isActiveAndEnabled)
            {
                _enemySpawner.StopSpawn();
                _agent.SetDestination(transform.position);
                _animator.SetFloat(_moveId, 0);
            }
        }

        private void Move()
        {
            if (_agent.isActiveAndEnabled)
            {
                _animator.SetFloat(_moveId, _agent.speed);
                _agent.SetDestination(_targetMove);
            }
        }
        
        private void OnDestroy()
        {
            _mainBuilding.OnDestroyBuilding -= OnDestroyBuilding;
        }
    }
}
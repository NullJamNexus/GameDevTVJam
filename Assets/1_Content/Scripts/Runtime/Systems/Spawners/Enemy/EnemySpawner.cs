using System;
using System.Collections.Generic;
using MEC;
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Factories;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Systems
{
    public enum EnemySpawnState
    {
        Disabled,
        Waiting,
        Spawning
    }
    
    public class EnemySpawner : IEnemySpawner, IDisposable
    {
        private ICharacterFactory _characterFactory;
        private List<EnemyController> _spawnedEnemies = new();
        private EnemySpawnState _spawnState = EnemySpawnState.Disabled;
        private CoroutineHandle _spawnerCoroutine;
        private float _timer;
        private float _spawnRate = 1f;
        private int _maxEnemies = 2;
        
        private Vector2[] _spawnPoints = new []
        {
            new Vector2(-7f, 0f),
            new Vector2(7f, 0f)
        };
        
        public EnemySpawner(ICharacterFactory characterFactory)
        {
            _characterFactory = characterFactory;
        }
        
        public void StartSpawner()
        {
            _spawnerCoroutine = Timing.RunCoroutine(SpawnerRunningCoroutine());
        }
        
        public void Dispose()
        {
            Timing.KillCoroutines(_spawnerCoroutine);
        }
        
        private void ChangeSpawnState(EnemySpawnState newState)
        {
            _spawnState = newState;
            
            switch (_spawnState)
            {
                case EnemySpawnState.Disabled:
                    break;
                case EnemySpawnState.Waiting:
                    break;
                case EnemySpawnState.Spawning:
                    break;
                default:
                    Debug.Log("[EnemySpawner] Invalid spawn state!");
                    break;
            }
        }
        
        private IEnumerator<float> SpawnerRunningCoroutine()
        {
            ChangeSpawnState(EnemySpawnState.Waiting);
        
            while (_spawnState != EnemySpawnState.Disabled && CanSpawnMoreEnemies())
            {
                ChangeSpawnState(EnemySpawnState.Spawning);
                SpawnEnemy();
                yield return Timing.WaitForSeconds(_spawnRate);
            }
            
            ChangeSpawnState(EnemySpawnState.Waiting);
        }
        
        private bool CanSpawnMoreEnemies()
        {
            return _spawnedEnemies.Count < _maxEnemies;
        }
        
        private void SpawnEnemy()
        {
            EnemyController enemy = _characterFactory.CreateEnemy();
            _spawnedEnemies.Add(enemy);
            enemy.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
        }
    }
}
﻿using System;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using NJN.Runtime.Systems;
using NJN.Runtime.Systems.Spawners;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class LevelManager : MonoBehaviour, ILevelStateHandler
    {
        [field: FoldoutGroup("Level Inventory"), SerializeField, HideLabel]
        public LevelInventory LevelInventory { get; private set; }

        [field: BoxGroup("Items"), SerializeField]
        private int _itemsToSpawn = 10;
        
        private ICharacterFactory _characterFactory;
        private IEnemySpawner _enemySpawner;
        private IItemSpawner _itemSpawner;
        private IInputProvider _inputProvider;
        private SignalBus _signalBus;
        
        public PlayerController Player { get; private set; }

        [Inject]
        private void Construct(ICharacterFactory characterFactory, IEnemySpawner enemySpawner, IItemSpawner itemSpawner,
            IInputProvider inputProvider, SignalBus signalBus)
        {
            _characterFactory = characterFactory;
            _enemySpawner = enemySpawner;
            _itemSpawner = itemSpawner;
            _inputProvider = inputProvider;
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<ResourceCollectedSignal>(OnResourceCollected);
        }

        private void Start()
        {
            SpawnPlayer();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.TryUnsubscribe<ResourceCollectedSignal>(OnResourceCollected);
        }
        
        [Button(ButtonSizes.Large)]
        private void SpawnPlayer()
        {
            if (Player != null)
                Player.transform.position = Vector2.zero;
            
            Player = _characterFactory.CreatePlayer();
            Player.transform.position = Vector2.zero;
            _inputProvider.EnablePlayerControls();
        }
        
        [Button(ButtonSizes.Large)]
        private void StartEnemySpawner()
        {
            _enemySpawner.StartSpawner();
        }
        
        [Button(ButtonSizes.Large)]
        private void SpawnItems()
        {
            for (int i = 0; i < _itemsToSpawn; i++)
                _itemSpawner.SpawnItem();
        }
        
        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            Destroy(Player.gameObject);
        }
        
        private void OnResourceCollected(ResourceCollectedSignal signal)
        {
            LevelInventory.AddResource(signal.Resource);
        }
    }
}
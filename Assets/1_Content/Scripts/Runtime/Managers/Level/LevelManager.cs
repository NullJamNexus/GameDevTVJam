using System;
using System.Collections.Generic;
using MEC;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Destination;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using NJN.Runtime.Managers.Level.Signals;
using NJN.Runtime.Managers.Signals;
using NJN.Runtime.Systems;
using NJN.Runtime.Systems.Spawners;
using NJN.Runtime.UI;
using NJN.Runtime.UI.Panels;
using NJN.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class LevelManager : MonoBehaviour, ILevelStateHandler
    {
        // [field: BoxGroup("Items"), SerializeField]
        // private int _itemsToSpawn = 10;
        [field: BoxGroup("Player"), SerializeField]
        private Vector2 _playerSpawnPosition = new (4.19f, 2.11f);

        [field: BoxGroup("Destinations"), SerializeField]
        private DestinationOptionSO _startingDestination;
        
        private ICharacterFactory _characterFactory;
        // TODO: Not using some of these spawners here...
        private IEnemySpawner _enemySpawner;
        private IItemSpawner _itemSpawner;
        private IDestinationsFactory _destinationsFactory;
        private IInputProvider _inputProvider;
        private SignalBus _signalBus;
        // TODO: Too many dependencies, should refactor if have time...
        private PlayerHUD _playerHUD;
        
        public PlayerController Player { get; private set; }
        public DestinationController CurrentDestination { get; private set; }

        [Inject]
        private void Construct(ICharacterFactory characterFactory, IEnemySpawner enemySpawner, IItemSpawner itemSpawner, 
            IDestinationsFactory destinationsFactory, IInputProvider inputProvider, SignalBus signalBus, [Inject(Id = "HUD")] PlayerHUD playerHUD)
        {
            _characterFactory = characterFactory;
            _enemySpawner = enemySpawner;
            _itemSpawner = itemSpawner;
            _destinationsFactory = destinationsFactory;
            _inputProvider = inputProvider;
            _signalBus = signalBus;
            _playerHUD = playerHUD;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<DestinationSelectedSignal>(OnDestinationSelected);
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnStartingDestination();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.TryUnsubscribe<DestinationSelectedSignal>(OnDestinationSelected);
        }
        
        [Button(ButtonSizes.Large)]
        private void SpawnPlayer()
        {
            if (Player != null)
            {
                // TODO: We don't really want to respawn the player, this is just for testing...
                Player.transform.position = Vector2.zero;
                Player.gameObject.SetActive(true);
                Player.StateMachine.ChangeState(Player.IdleState);
                return;
            }

            Player = _characterFactory.CreatePlayer();
            _playerHUD.SetUp(Player.Stats);
            Player.transform.position = _playerSpawnPosition;
            _inputProvider.EnablePlayerControls();
        }
        
        private void SpawnStartingDestination()
        {
            CurrentDestination = _destinationsFactory.CreateDestination(_startingDestination);
        }
        
        // [Button(ButtonSizes.Large)]
        // private void StartEnemySpawner()
        // {
        //     _enemySpawner.StartSpawner();
        // }
        //
        // [Button(ButtonSizes.Large)]
        // private void SpawnItems()
        // {
        //     for (int i = 0; i < _itemsToSpawn; i++)
        //         _itemSpawner.SpawnItem();
        // }
        
        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            Player.gameObject.SetActive(false);
            _inputProvider.EnableUIControls();
            Timing.RunCoroutine(PlayerDiedCoroutine().CancelWith(this));
        }
        
        private IEnumerator<float> PlayerDiedCoroutine()
        {
            Debug.Log("PLAYER DIED, GOING TO MAIN MENU...");
            
            yield return Timing.WaitForSeconds(1f);
            
            _signalBus.Fire<GameOverSignal>();
        }
        
        private void OnDestinationSelected(DestinationSelectedSignal signal)
        {
            _signalBus.Fire<DestinationTransitionStartedSignal>();

            Timing.RunCoroutine(TransitionCoroutine(signal.DestinationData).CancelWith(this));
        }
        
        private IEnumerator<float> TransitionCoroutine(DestinationOptionSO destinationData)
        {
            _inputProvider.EnableUIControls();
            
            // CurrentDestination.MoveOldDestination(0.5f, 3f);
            
            yield return Timing.WaitForSeconds(1f);
            
            Destroy(CurrentDestination.gameObject);

            yield return Timing.WaitForSeconds(3f);
            
            CurrentDestination = _destinationsFactory.CreateDestination(destinationData);
            
            // CurrentDestination.MoveNewDestination(3f, 0.5f);

            yield return Timing.WaitForSeconds(1f);
            
            _inputProvider.EnablePlayerControls();
            _signalBus.Fire<DestinationTransitionFinishedSignal>();
        }
    }
}
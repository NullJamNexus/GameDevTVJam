using System.Collections.Generic;
using MEC;
using NJN.Runtime.Controllers.Destination;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using NJN.Runtime.Managers.Level.Signals;
using NJN.Runtime.Managers.Signals;
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
        [BoxGroup("Player"), SerializeField]
        private Vector2 _playerSpawnPosition = new (4.19f, 2.11f);

        [BoxGroup("Destinations"), SerializeField]
        private DestinationOptionSO _startingDestination;
        [BoxGroup("Destinations"), SerializeField]
        private DestinationOptionSO _endingDestination;
        [BoxGroup("Destinations"), SerializeField]
        private int _totalDestinations = 10;

        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _parallaxMaxSpeed = 3f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _parallaxAccelerationRate = 0.5f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _buildingMaxSpeed = 3f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _buildingAccelerationRate = 0.5f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _buildingRemoveTime = 3f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _truckTravelTime = 3f;
        [FoldoutGroup("Destination Transition"), SerializeField, ReadOnly, Tooltip("DO NOT CHANGE")]
        private float _buildingSpawnX = -10f;
        [FoldoutGroup("Destination Transition"), SerializeField, ReadOnly, Tooltip("DO NOT CHANGE")]
        private float _buildingDecelerationRate = 0.5f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _arrivalTravelTime = 3f;
        [FoldoutGroup("Destination Transition"), SerializeField]
        private float _parallaxDecelerationRate = 0.5f;

        private int _currentDestinationCount = 0;

        private ICharacterFactory _characterFactory;
        private IDestinationsFactory _destinationsFactory;
        private IInputProvider _inputProvider;
        private SignalBus _signalBus;
        private PlayerHUD _playerHUD;

        public PlayerController Player { get; private set; }
        public DestinationController CurrentDestination { get; private set; }

        [Inject]
        private void Construct(ICharacterFactory characterFactory, IDestinationsFactory destinationsFactory,
            IInputProvider inputProvider, SignalBus signalBus, [Inject(Id = "HUD")] PlayerHUD playerHUD)
        {
            _characterFactory = characterFactory;
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

        private void SpawnPlayer()
        {
            if (Player != null)
            {
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
            CurrentDestination.SetActiveStateForEntities(true);
        }

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
            _signalBus.Fire<GameLostSignal>();
        }

        private void OnDestinationSelected(DestinationSelectedSignal signal)
        {
            _currentDestinationCount++;
            _signalBus.Fire(new ProgressUpdatedSignal(_currentDestinationCount, _totalDestinations));
            _signalBus.Fire(new DestinationTransitionStartedSignal(_parallaxMaxSpeed, _parallaxAccelerationRate));
            Timing.RunCoroutine(TransitionCoroutine(signal.DestinationData).CancelWith(this));
        }

        private IEnumerator<float> TransitionCoroutine(DestinationOptionSO destinationData)
        {
            bool isEnding = _currentDestinationCount >= _totalDestinations;

            /*_inputProvider.EnableUIControls();*/
            //Maybe Hide the truck Ladder.

            CurrentDestination.MoveOldHouse(_buildingAccelerationRate, _buildingMaxSpeed);

            yield return Timing.WaitForSeconds(_buildingRemoveTime);

            Destroy(CurrentDestination.gameObject);

            yield return Timing.WaitForSeconds(_truckTravelTime);

            DestinationOptionSO destination = isEnding ? _endingDestination : destinationData;
            CurrentDestination = _destinationsFactory.CreateDestination(destination);
            CurrentDestination.transform.position = new Vector2(_buildingSpawnX, 0f);
            CurrentDestination.MoveNewHouse(_buildingMaxSpeed, _buildingDecelerationRate);

            yield return Timing.WaitForSeconds(_arrivalTravelTime);

            _signalBus.Fire(new DestinationTransitionFinishedSignal(_parallaxDecelerationRate));

            // TODO: In the final destination, we can make it so when you walk into house trigger, you win game instead of here...
            if (isEnding)
                _signalBus.Fire(new GameWonSignal());

            _inputProvider.EnablePlayerControls();
        }
    }
}

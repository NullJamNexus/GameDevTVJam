using System;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.Input;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerController : BaseCharacterController
    {
        public IInputProvider InputProvider { get; private set; }
        public CinemachineCamera Camera { get; private set; }

        private SignalBus _signalBus;
        
        #region Components

        public IMovement Movement { get; private set; }
        public ISurvivalStats Stats { get; private set; }
        public IInteractor Interactor { get; private set; }

        #endregion
        
        #region States

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerClimbState ClimbState {get; private set;}
        public CharacterBusyState BusyState { get; private set; }
        public CharacterDeadState DeadState { get; private set; }

        #endregion

        [Inject]
        private void Construct(IInputProvider inputProvider, SignalBus signalBus,
            [Inject(Id = "FollowCamera")] CinemachineCamera vmCamera)
        {
            InputProvider = inputProvider;
            _signalBus = signalBus;
            Camera = vmCamera;
        }
        
        protected override void InitializeStateMachine()
        {
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            ClimbState = new PlayerClimbState(this, StateMachine);
            BusyState = new CharacterBusyState(this, StateMachine);
            DeadState = new CharacterDeadState(this, StateMachine);
            
            StateMachine.Initialize(BusyState);
        }

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();

            Movement = VerifyComponent<IMovement>();
            Stats = VerifyComponent<ISurvivalStats>();
            Interactor = VerifyComponent<IInteractor>();
        }

        private void OnEnable()
        {
            Stats.DiedEvent += OnDeath;
        }

        private void Start()
        {
            Camera.Follow = transform;
            StateMachine.ChangeState(IdleState);
        }

        private void OnDisable()
        {
            Stats.DiedEvent -= OnDeath;
        }

        #endregion
        
        private void OnDeath()
        {
            _signalBus.Fire(new PlayerDiedSignal());
        }
    }
}
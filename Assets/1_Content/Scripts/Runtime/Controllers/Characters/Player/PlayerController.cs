using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player.Dead;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.Input;
using NJN.Runtime.Managers;
using Unity.Cinemachine;
using Zenject;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerController : BaseCharacterController
    {
        public IInputProvider InputProvider { get; private set; }
        public CinemachineCamera Camera { get; private set; }

        public SignalBus SignalBus { get; private set; }
        
        #region Components

        public IMovement Movement { get; private set; }
        public ISurvivalStats Stats { get; private set; }
        public IInteractor Interactor { get; private set; }
        public IHideProcessor HideProcessor { get; private set; }

        #endregion
        
        #region States
        // Active
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerClimbState ClimbState {get; private set;}
        // Busy
        public CharacterBusyState BusyState { get; private set; }
        public PlayerHideState HideState { get; private set; }
        // Dead
        public PlayerDeadState DeadState { get; private set; }

        #endregion

        [Inject]
        private void Construct(IInputProvider inputProvider, SignalBus signalBus,
            [Inject(Id = "FollowCamera")] CinemachineCamera vmCamera)
        {
            InputProvider = inputProvider;
            SignalBus = signalBus;
            Camera = vmCamera;
        }
        
        protected override void InitializeStateMachine()
        {
            // Active
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            ClimbState = new PlayerClimbState(this, StateMachine);
            // Busy
            BusyState = new CharacterBusyState(this, StateMachine);
            HideState = new PlayerHideState(this, StateMachine);
            // Dead
            DeadState = new PlayerDeadState(this, StateMachine);
            
            StateMachine.Initialize(BusyState);
        }

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();

            Movement = VerifyComponent<IMovement>();
            Stats = VerifyComponent<ISurvivalStats>();
            Interactor = VerifyComponent<IInteractor>();
            HideProcessor = VerifyComponent<IHideProcessor>();
        }

        private void OnEnable()
        {
            SignalBus.Subscribe<PlayerDamageSignal>(OnGlobalDamage);
            SignalBus.Subscribe<PlayerTeleportSignal>(OnTeleported);
            SignalBus.Subscribe<PlayerHideSignal>(OnHide);
            
            Stats.DiedEvent += OnDeath;
        }

        private void Start()
        {
            Camera.Follow = transform;
            StateMachine.ChangeState(IdleState);
        }

        private void OnDisable()
        {
            SignalBus.TryUnsubscribe<PlayerDamageSignal>(OnGlobalDamage);
            SignalBus.TryUnsubscribe<PlayerTeleportSignal>(OnTeleported);
            SignalBus.TryUnsubscribe<PlayerHideSignal>(OnHide);
            
            Stats.DiedEvent -= OnDeath;
        }

        #endregion
        
        public override void TakeDamage(float damage)
        {
            Feedbacks.HitFeedback.PlayFeedbacks();
            Stats.TakeDamage(damage);
        }
        
        private void OnGlobalDamage(PlayerDamageSignal signal)
        {
            TakeDamage(signal.Amount);
        }
        
        private void OnTeleported(PlayerTeleportSignal signal)
        {
            transform.position = signal.NewPosition;
        }
        
        private void OnHide(PlayerHideSignal signal)
        {
            if (StateMachine.CurrentState != HideState)
                StateMachine.ChangeState(HideState);
        }
        
        private void OnDeath()
        {
            StateMachine.ChangeState(DeadState);
        }
    }
}
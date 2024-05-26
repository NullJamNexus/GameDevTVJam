using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerController : BaseCharacterController
    {
        public IInputProvider InputProvider { get; private set; }

        #region Components

        public IMovement Movement { get; private set; }

        #endregion
        
        #region States

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerClimbState ClimbState {get; private set;}
        public CharacterBusyState BusyState { get; private set; }
        public CharacterDeadState DeadState { get; private set; }

        #endregion

        [Inject]
        private void Construct(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
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
        }
        
        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }

        #endregion

        //Check layer of ladder and initiate climbing
        
    }
}
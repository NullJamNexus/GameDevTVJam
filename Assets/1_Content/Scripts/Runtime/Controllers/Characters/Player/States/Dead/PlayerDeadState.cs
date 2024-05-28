using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.Player.Dead
{
    public class PlayerDeadState : CharacterDeadState
    {
        protected PlayerController _player;
        
        public PlayerDeadState(PlayerController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _player = controller;
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.SignalBus.Fire(new PlayerDiedSignal());
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerMoveState : PlayerActiveState
    {
        private bool _isSprinting;
        
        public PlayerMoveState(PlayerController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _isSprinting = _player.InputProvider.SprintInput.Held;
            
            if (ShouldIdle())
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Vector2 horizontalMove = new (_player.InputProvider.MoveInput.x, 0f);
            _player.Movement.PhysicsHorizontalMove(horizontalMove, _isSprinting);
        }

        public override void Exit()
        {
            base.Exit();
            
            _player.Movement.PhysicsStop();
        }
        
        private bool ShouldIdle()
        {
            return _player.InputProvider.MoveInput.x == 0;
        }
    }
}
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerMoveState : CharacterActiveState
    {
        private PlayerController _player;
        
        public PlayerMoveState(PlayerController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _player = controller;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Vector2 horizontalMove = new (_player.InputProvider.MoveInput.x, 0f);
            _player.Movement.Move(horizontalMove, _player.MovementSpeed);
            
            if (ShouldIdle())
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        private bool ShouldIdle()
        {
            return _player.InputProvider.MoveInput == Vector2.zero;
        }
    }
}
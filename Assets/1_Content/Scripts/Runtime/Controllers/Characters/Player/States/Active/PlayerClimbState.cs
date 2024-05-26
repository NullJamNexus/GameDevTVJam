using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerClimbState : PlayerActiveState
    {
        private float _originalGravity;
        
        public PlayerClimbState(PlayerController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _originalGravity = _player.Rigidbody.gravityScale;
            _player.Rigidbody.gravityScale = 0f;
            _player.Collider.isTrigger = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Vector2 climbInput = new (0f, _player.InputProvider.MoveInput.y);
            _player.Movement.Move(climbInput, _player.MovementSpeed);
            
            if (ShouldIdle())
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void OnTriggerExit(Collider2D collider)
        {
            base.OnTriggerExit(collider);
            
            if (collider.TryGetComponent(out IClimbable climbable))
            {
                _stateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _player.Rigidbody.gravityScale = _originalGravity;
            _player.Collider.isTrigger = false;
        }
        
        private bool ShouldIdle()
        {
            return _player.InputProvider.MoveInput.y == 0;
        }
    }
}
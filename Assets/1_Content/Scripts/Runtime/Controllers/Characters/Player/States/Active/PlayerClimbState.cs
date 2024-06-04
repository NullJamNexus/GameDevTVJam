using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerClimbState : PlayerActiveState
    {
        private float _originalGravity = 666;
        
        public PlayerClimbState(PlayerController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if(_originalGravity == 666) 
                _originalGravity = _player.Rigidbody.gravityScale;
            _player.Rigidbody.gravityScale = 0f;
            _player.Collider.isTrigger = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            
            
            if (!ShouldClimb(_player.Movement.Climbable))
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Vector2 climbInput = new (0f, _player.InputProvider.MoveInput.y);
            _player.Movement.PhysicsMove(climbInput, false);

            if(climbInput.y != 0)
            {
                _player.SignalBus.Fire<PlayerClimbSignal>();
            }
            else
            {
                _player.SignalBus?.Fire<PlayerEndClimbSignal>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _player.Movement.PhysicsStop();
            _player.Rigidbody.gravityScale = _originalGravity;
            _player.Collider.isTrigger = false;

            _player.SignalBus?.Fire<PlayerEndClimbSignal>();

        }
    }
}
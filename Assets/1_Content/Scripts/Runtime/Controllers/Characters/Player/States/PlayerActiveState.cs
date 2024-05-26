using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerActiveState : CharacterActiveState
    {
        protected PlayerController _player;
        
        public PlayerActiveState(PlayerController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _player = controller;
        }

        public override void Enter()
        {
            base.Enter();

            _player.StateName = _stateMachine.CurrentState.GetType().Name;
        }

        public override void OnTriggerEnter(Collider2D collider)
        {
            base.OnTriggerEnter(collider);
            
            if (collider.gameObject.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect(_player);
            }
        }

        public override void OnTriggerStay(Collider2D collider)
        {
            base.OnTriggerStay(collider);
            
            if (ShouldClimb() && collider.gameObject.TryGetComponent(out IClimbable climbable))
            {
                _player.StateMachine.ChangeState(_player.ClimbState);
            }
        }
        
        protected virtual bool ShouldClimb()
        {
            if (_stateMachine.CurrentState == _player.ClimbState)
                return false;
            
            return _player.InputProvider.MoveInput.y != 0;
        }
    }
}
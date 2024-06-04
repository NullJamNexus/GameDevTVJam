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

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldInteract())
            {
                _player.Interactor.Interact();
                return;
            }
            
            if (_stateMachine.CurrentState != _player.ClimbState && ShouldClimb(_player.Movement.Climbable))
            {
                _stateMachine.ChangeState(_player.ClimbState);
                return;
            }
        }

        public override void OnTriggerEnter(Collider2D collider)
        {
            base.OnTriggerEnter(collider);
            
            if (collider.gameObject.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect(_player.Stats);
            }
            else if (collider.gameObject.TryGetComponent(out IClimbable climbable))
            {
                _player.Movement.Climbable = climbable;
            }
        }
        
        public override void OnTriggerExit(Collider2D collider)
        {
            base.OnTriggerExit(collider);
            
            if (collider.TryGetComponent(out IClimbable climbable) && climbable == _player.Movement.Climbable)
            {
                _player.Movement.Climbable = null;
            }
        }
        
        public override void Exit()
        {
            base.Exit();

            _player.StateName = "";
        }
        
        protected virtual bool ShouldInteract()
        {
            if (!_player.InputProvider.InteractInput.Pressed)
                return false;

            return _player.Interactor.Interactable != null;
        }
        
        protected virtual bool ShouldClimb(IClimbable climbable)
        {
            if (climbable == null)
                return false;
            
            float moveInputY = _player.InputProvider.MoveInput.y;
            float playerPosY = _player.transform.position.y;
            float climbableTopY = climbable.GetTop().y;
            float climbableBottomY = climbable.GetBottom().y;

            if (moveInputY > 0 && playerPosY >= climbableTopY)
                return false;

            if (moveInputY < 0 && playerPosY <= climbableBottomY)
                return false;

            return moveInputY != 0;
        }
    }
}
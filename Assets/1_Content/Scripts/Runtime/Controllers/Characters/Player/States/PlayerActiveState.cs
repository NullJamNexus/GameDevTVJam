using ModestTree;
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
    //attempting to stop player from clipping through floor when climbing
            if(collider.gameObject.layer==3){
                _player.StateMachine.ChangeState(_player.IdleState);
                _player.transform.position = new Vector2(_player.transform.position.x,_player.transform.position.y+0.2f);
                Debug.Log("Stopped climbing due to touching an obstacle");
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
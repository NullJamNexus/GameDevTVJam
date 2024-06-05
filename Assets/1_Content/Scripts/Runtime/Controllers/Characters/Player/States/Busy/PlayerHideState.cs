using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public class PlayerHideState : CharacterBusyState
    {
        protected PlayerController _player;
        private Color _defaultColor;
        private string _defaultLayer;
        
        public PlayerHideState(PlayerController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _player = controller;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _player.StateName = _stateMachine.CurrentState.GetType().Name;
            
            _player.SignalBus.Subscribe<PlayerUnhideSignal>(OnUnhide);

            _defaultColor = _player.Model.color;
            Color color = _player.Model.color;
            color.a = _player.HideProcessor.HiddenAlpha;
            _player.Model.color = color;
            _player.Collider.isTrigger = true;
            _defaultLayer = LayerMask.LayerToName(_player.gameObject.layer);
            _player.gameObject.layer = LayerMask.NameToLayer(_player.HideProcessor.GetHiddenLayer());
            _player.Rigidbody.isKinematic = true;

            _player.Movement.PhysicsStop();
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldInteract())
            {
                _player.Interactor.Interact();
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            _player.SignalBus.Unsubscribe<PlayerUnhideSignal>(OnUnhide);
            
            _player.Model.color = _defaultColor;
            _player.Collider.isTrigger = false;
            _player.gameObject.layer = LayerMask.NameToLayer(_defaultLayer);
            _player.Rigidbody.isKinematic = false;
        }
        
        protected virtual bool ShouldInteract()
        {
            if (!_player.InputProvider.InteractInput.Pressed)
                return false;

            return _player.Interactor.Interactable != null;
        }
        
        private void OnUnhide(PlayerUnhideSignal signal)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}
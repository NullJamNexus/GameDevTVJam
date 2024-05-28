using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyDistractedState : CharacterBusyState
    {
        protected EnemyController _enemy;
        private Vector2 _direction;
        
        public EnemyDistractedState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _enemy = controller;
        }

        public override void Enter()
        {
            base.Enter();

            GetDirection();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldIdle())
            {
                _stateMachine.ChangeState(_enemy.IdleState);
                return;
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            _enemy.Movement.PhysicsHorizontalMove(_direction, true);
        }

        public override void Exit()
        {
            base.Exit();
            
            _enemy.Movement.PhysicsStop();
        }
        
        private void GetDirection()
        {
            Vector2 target = _enemy.DistractionProcessor.DistractionLocation;
            Vector2 aiPosition = _enemy.transform.position;
            Vector2 direction = target - aiPosition;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;
            _enemy.Flip(_direction);
        }
        
        private bool ShouldIdle()
        {
            float xDistance = Mathf.Abs(_enemy.DistractionProcessor.DistractionLocation.x - _enemy.transform.position.x);
            return xDistance < _enemy.DistractionProcessor.CloseEnough;
        }
    }
}
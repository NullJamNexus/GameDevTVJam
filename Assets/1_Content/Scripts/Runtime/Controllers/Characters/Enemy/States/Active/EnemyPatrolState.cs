using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyPatrolState : EnemyActiveState
    {
        private Vector2 _direction;
        private Vector2 _patrolDestination;
        
        public EnemyPatrolState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            SetMoveDirection();

            _enemy.Animator.SetBool(_enemy.AnimParams.RunBoolName, true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ShouldIdle())
            {
                _stateMachine.ChangeState(_enemy.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            _enemy.Movement.PhysicsHorizontalMove(_direction, false);
        }

        public override void Exit()
        {
            base.Exit();
            
            _enemy.Movement.PhysicsStop();
            _enemy.Animator.SetBool(_enemy.AnimParams.RunBoolName, false);
        }

        private void SetMoveDirection()
        {
            Vector2 direction = _enemy.Patrol.GetNextPointDirection(_enemy.transform.position);
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;
            _enemy.Flip(_direction);
        }
        
        private bool ShouldIdle()
        {
            return _enemy.Patrol.HasArrivedAtPoint(_enemy.transform.position);
        }
    }
}
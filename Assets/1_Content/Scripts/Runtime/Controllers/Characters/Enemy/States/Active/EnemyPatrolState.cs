using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyPatrolState : EnemyActiveState
    {
        private float _closeEnough = 0.5f;
        private int _patrolPointIndex = 0;
        private Vector2 _direction;
        private Vector2 _patrolDestination;
        
        public EnemyPatrolState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (_enemy.PatrolPoints.Count > 0)
                GetMoveDirection();
            else
                _stateMachine.ChangeState(_enemy.IdleState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _enemy.Movement.Move(_direction, _enemy.PatrolSpeed);

            if (ShouldIdle())
            {
                _patrolPointIndex = (_patrolPointIndex + 1) % _enemy.PatrolPoints.Count;
                _stateMachine.ChangeState(_enemy.IdleState);
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
        public override void HasLineOfSightToPlayer()
        {
            base.HasLineOfSightToPlayer();
            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
        }
        public override void TryToDistract()
        {
            base.TryToDistract();
            _enemy.StateMachine.ChangeState(_enemy.DistractedState);
        }

        private void GetMoveDirection()
        {
            _patrolDestination = _enemy.PatrolPoints[_patrolPointIndex];
            Vector2 direction = _patrolDestination - (Vector2)_enemy.transform.position;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;
            _enemy.ChangeFaceDirection(direction.x);
        }
        
        private bool ShouldIdle()
        {
            float distance = Mathf.Abs(_patrolDestination.x - _enemy.transform.position.x);
            
            if (distance < _closeEnough)
                return true;

            return false;
        }
    }
}
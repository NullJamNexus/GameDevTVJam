using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyChaseState : EnemyActiveState
    {
        private Vector2 _direction;
        
        public EnemyChaseState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _enemy.SignalBus.Fire(new EnemyChaseSignal());
            _enemy.Animator.SetBool(_enemy.AnimParams.RunBoolName, true);

        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldIdle())
            {
                _enemy.SignalBus.Fire(new EndEnemyChaseSignal());
                _stateMachine.ChangeState(_enemy.IdleState);
                return;
            }
            
            SetDirection();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            _enemy.Movement.PhysicsHorizontalMove(_direction, true);
        }
        
        private void SetDirection()
        {
            IDamagable target = _enemy.LineOfSight.DamagableTarget;
            Vector3 aiPosition = _enemy.transform.position;
            Vector2 direction = target != null ? target.Transform.position - aiPosition : Vector2.zero;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;
            _enemy.Flip(_direction);
        }

        public override void Exit()
        {
            base.Exit();
            
            _enemy.Movement.PhysicsStop();
            _enemy.Animator.SetBool(_enemy.AnimParams.RunBoolName, false);
        }
        
        private bool ShouldIdle()
        {
            return _enemy.LineOfSight.DamagableTarget == null;
        }
    }
}
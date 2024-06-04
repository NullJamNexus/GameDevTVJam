using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyActiveState : CharacterState
    {
        protected EnemyController _enemy;
        private float _losCheckTimer;
        
        public EnemyActiveState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _enemy = controller;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _enemy.StateName = _stateMachine.CurrentState.GetType().Name;
            
            CheckForTarget();
            
            if (ShouldAttack())
            {
                _stateMachine.ChangeState(_enemy.AttackState);
                return;
            }
            else if (ShouldChase())
            {
                _stateMachine.ChangeState(_enemy.ChaseState);
                return;
            }
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            base.OnCollisionEnter(collision);
            
            if (collision.gameObject.TryGetComponent(out IDamagable damagable))
            {
                _enemy.DamageProcessor.DealDamage(damagable);
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(_enemy.GetComponent<Collider2D>(), collision.collider);
            }
        }

        public override void Exit()
        {
            base.Exit();

            _enemy.StateName = "";
        }

        private void CheckForTarget()
        {
            if (_losCheckTimer + _enemy.LineOfSight.CheckInterval > Time.time)
                return;
            
            _losCheckTimer = Time.time;
            
            Vector2 direction = _enemy.IsFacingRight ? Vector2.right : Vector2.left;
            _enemy.LineOfSight.DamagableTarget = 
                _enemy.LineOfSight.TryGetVisibleDamagable(direction, out IDamagable damagable) ? damagable : null;
        }
        
        private bool ShouldAttack()
        {
            if (_stateMachine.CurrentState == _enemy.AttackState || _enemy.LineOfSight.DamagableTarget == null)
                return false;
            
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.LineOfSight.DamagableTarget.Transform.position);
            if (_enemy.DamageProcessor.AttackRange < distance)
                return false;
            
            return _enemy.LineOfSight.DamagableTarget != null;
        }
        
        private bool ShouldChase()
        {
            return _stateMachine.CurrentState != _enemy.AttackState
                   && _stateMachine.CurrentState != _enemy.ChaseState
                   && _enemy.LineOfSight.DamagableTarget != null;
        }
    }
}
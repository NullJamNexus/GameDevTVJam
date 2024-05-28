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
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            base.OnCollisionEnter(collision);
            
            if (collision.gameObject.TryGetComponent(out IDamagable damagable))
            {
                _enemy.DamageProcessor.DealDamage(damagable);
            }
        }

        public override void Exit()
        {
            base.Exit();

            _enemy.StateName = "";
        }

        private void CheckForTarget()
        {
            if (_losCheckTimer + _enemy.LineOfSight.CheckInterval > Time.deltaTime)
                return;
            
            _losCheckTimer = Time.deltaTime;
            
            Vector2 direction = _enemy.IsFacingRight ? Vector2.right : Vector2.left;
            if (_enemy.LineOfSight.TryGetVisibleDamagable(direction, out IDamagable damagable))
            {
                _enemy.LineOfSight.DamagableTarget = damagable;
                _stateMachine.ChangeState(_enemy.ChaseState);
            }
            else
            {
                _enemy.LineOfSight.DamagableTarget = null;
            }
        }
    }
}
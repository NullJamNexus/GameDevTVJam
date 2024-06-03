using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyAttackState : EnemyActiveState
    {
        // TODO: This will be dictated by animator.
        private float _attackDuration = 0.5f;
        private float _attackTimer;
        
        public EnemyAttackState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _attackTimer = Time.time;
            // TODO: Timing of damage might need to change
            if (_enemy.LineOfSight.DamagableTarget != null)
            {
                _enemy.Animator.SetTrigger(_enemy.AnimParams.AttackTriggerName);
                _enemy.DamageProcessor.DealDamage(_enemy.LineOfSight.DamagableTarget);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldChase())
            {
                _stateMachine.ChangeState(_enemy.ChaseState);
                return;
            }
        }
        
        private bool ShouldChase()
        {
            return Time.time >= _attackTimer + _attackDuration;
        }
    }
}
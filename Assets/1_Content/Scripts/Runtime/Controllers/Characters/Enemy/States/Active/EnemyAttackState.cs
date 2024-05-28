using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using System;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyAttackState : EnemyActiveState
    {
        bool _isAttacking;
        public EnemyAttackState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // _enemy.Attack.StartAttack(AttackEnded);
            _isAttacking = true;
        }

        private void AttackEnded()
        {
            _isAttacking = false;
            _enemy.StateMachine.ChangeState(_enemy.IdleState);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // _enemy.Attack.UpdateAttack();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            if (_isAttacking)
            {
                _isAttacking = false;
                // _enemy.Attack.CancellAttack();
            }
        }
    }
}
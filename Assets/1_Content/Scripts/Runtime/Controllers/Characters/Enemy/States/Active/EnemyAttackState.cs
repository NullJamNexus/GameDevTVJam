using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using System;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyAttackState : EnemyActiveState
    {
        private IDamagable _target;
        private float _hitTime = 0.3f;
        private float _recoveryTime = 0.3f;
        private float _timeCounter;
        private Action _updateFunction;//it is called every update, changed after hit is made
        public EnemyAttackState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _updateFunction();       
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public void AttackTarget(IDamagable target)
        {
            _updateFunction = PreAttack;
            _target = target;
            _timeCounter = 0;
            _enemy.StateMachine.ChangeState(this);
        }

        private void HitDamagable()
        {
            _enemy.DamageProcessor.DealDamage(_target, _enemy.BaseDamage);
        }

        private void PreAttack()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter > _hitTime)
            {
                HitDamagable();
                _timeCounter = 0;
                _updateFunction = AfterAttack;
            }
        }

        private void AfterAttack()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter > _recoveryTime)
            {
                _enemy.StateMachine.ChangeState(_enemy.IdleState);
            }
        }
    }
}
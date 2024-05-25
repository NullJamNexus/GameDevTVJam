using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyActiveState : CharacterState
    {
        protected EnemyController _enemy;
        
        public EnemyActiveState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _enemy = controller;
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            base.OnCollisionEnter(collision);
            
            if (collision.gameObject.TryGetComponent(out IDamagable damagable))
            {
                _enemy.DamageProcessor.DealDamage(damagable, _enemy.BaseDamage);
            }
        }
    }
}
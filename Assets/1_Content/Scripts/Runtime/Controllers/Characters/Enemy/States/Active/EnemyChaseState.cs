using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyChaseState : EnemyActiveState
    {
        public EnemyChaseState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.Chase.StartChase();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            _enemy.Chase.UpdateChase();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            _enemy.Movement.PhysicsHorizontalMove(_enemy.Chase.Direction, false);
        }

        public override void Exit()
        {
            base.Exit();
            _enemy.Chase.CancellChase();
        }

    }
}
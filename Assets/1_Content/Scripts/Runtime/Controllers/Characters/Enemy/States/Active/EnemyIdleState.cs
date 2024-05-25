using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyIdleState : EnemyActiveState
    {
        public EnemyIdleState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldPatrol())
            {
                _stateMachine.ChangeState(_enemy.PatrolState);
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
        
        private bool ShouldPatrol()
        {
            return true;
        }
    }
}
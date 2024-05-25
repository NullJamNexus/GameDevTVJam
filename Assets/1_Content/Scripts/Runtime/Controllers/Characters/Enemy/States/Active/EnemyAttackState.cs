using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyAttackState : EnemyActiveState
    {
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
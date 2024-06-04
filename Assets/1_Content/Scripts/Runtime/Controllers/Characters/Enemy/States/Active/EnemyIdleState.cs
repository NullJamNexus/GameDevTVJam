using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyIdleState : EnemyActiveState
    {
        public EnemyIdleState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldPatrol())
            {
                _stateMachine.ChangeState(_enemy.PatrolState);
                return;
            }
        }

        private bool ShouldPatrol()
        {
            return true;
        }
    }
}
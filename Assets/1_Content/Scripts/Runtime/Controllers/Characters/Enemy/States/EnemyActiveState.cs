using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyActiveState : CharacterState
    {
        protected EnemyController _enemy;
        
        public EnemyActiveState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
            _enemy = controller;
        }
    }
}
using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyDistractedState : EnemyActiveState
    {        
        public EnemyDistractedState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            //_enemy.Distractable.StartDistraction(DistractionEnded);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            //_enemy.Distractable.UpdateLogic();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void DistractionEnded()
        {
            _enemy.StateMachine.ChangeState(_enemy.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            //_enemy.Distractable.CancellDistraction();
        }
        public override void HasLineOfSightToPlayer()
        {
            base.HasLineOfSightToPlayer();
            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
        }
    }
}
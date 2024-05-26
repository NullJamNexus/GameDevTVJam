using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyDistractedState : EnemyActiveState
    {
        private float _closeEnough = 0.2f;

        private Vector3 _distractionPosition;
        private Vector2 _direction;
        private float _distractionTime;
        
        public EnemyDistractedState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            GetMoveDirection();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            

            if (HasReachedDistractionPosition())
            {
                _distractionTime -= Time.deltaTime;
                if (_distractionTime <= 0)
                {
                    _stateMachine.ChangeState(_enemy.IdleState);
                }
            }
            else
            {
                _enemy.Movement.Move(_direction, _enemy.PatrolSpeed);
            }
        }

        public void Distraction(Vector3 position, float distractionTime)
        {
            _distractionPosition = position;
            _distractionTime = distractionTime;

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        private void GetMoveDirection()
        {
            Vector2 direction = (Vector2)_distractionPosition - (Vector2)_enemy.transform.position;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;

            E_FaceDirection enemyDirection = direction.x > 0 ? E_FaceDirection.right : E_FaceDirection.left;
            _enemy.ChangeFaceDirection(enemyDirection);
        }
        
        private bool HasReachedDistractionPosition()
        {
            float distance = Mathf.Abs(_distractionPosition.x - _enemy.transform.position.x);
            
            if (distance < _closeEnough)
                return true;

            return false;
        }
    }
}
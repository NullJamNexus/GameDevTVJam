using NJN.Runtime.Controllers.States;
using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyChaseState : EnemyActiveState
    {
        private float _attackRange = 1f;
        private GameObject _chaseTarget;
        private IDamagable _attackTarget;
        private Vector2 _direction;
        private float _playerLostWaitTime = 2; //after line of sight is lost, enemy will go to last player location and wait this amount of time
        public EnemyChaseState(EnemyController controller, ControllerStateMachine<CharacterState, BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }

        public void StartChasing(GameObject Player)
        {
            _attackTarget = Player.GetComponent<IDamagable>();

            if(_attackTarget == null)
            {
                Debug.Log("Chase object does not have IDamagable component");
                _enemy.StateMachine.ChangeState(_enemy.IdleState);
            }

            _chaseTarget = Player;
            _enemy.StateMachine.ChangeState(this);
        }

        public void LostLineOfSightToPlayer(Vector3 LastKnownPosition)
        {
            _enemy.DistractedState.Distraction(LastKnownPosition, _playerLostWaitTime);
            _enemy.StateMachine.ChangeState(_enemy.DistractedState);
        }
        public override void Enter()
        {
            base.Enter();
            if( _attackTarget == null || _chaseTarget == null) 
            {   
                Debug.Log("Chase object or IDamagable component is null");
                _enemy.StateMachine.ChangeState(_enemy.IdleState);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(IsInAttackRange())
            {
                _enemy.AttackState.AttackTarget(_attackTarget);
            }
            else
            {
                Move();
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

        private void Move()
        {
            GetMoveDirection();
            _enemy.Movement.Move(_direction, _enemy.PatrolSpeed);
        }
        private void GetMoveDirection()
        {
            Vector2 direction = (Vector2)_chaseTarget.transform.position - (Vector2)_enemy.transform.position;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;

            E_FaceDirection enemyDirection = direction.x > 0 ? E_FaceDirection.right : E_FaceDirection.left;
            _enemy.ChangeFaceDirection(enemyDirection);
        }

        private bool IsInAttackRange()
        {
            Vector2 distance = (Vector2)(_chaseTarget.transform.position - _enemy.transform.position);
            if (distance.magnitude < _attackRange)
                return true;
            return false;
        }

    }
}
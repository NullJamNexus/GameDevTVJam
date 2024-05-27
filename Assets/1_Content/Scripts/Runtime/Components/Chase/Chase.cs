using Mono.CSharp;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Enemy;
using System;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Chase : BaseComponent, IChase
    {
        EnemyController _enemyController;
        Vision _vision;

        [SerializeField] float _NecesseryDistance;
        private Vector2 _direction;
        private float _playerLostWaitTime = 2;
        private bool _isChasing;
        private Transform _target;
        private Vector3 _targetPos;
        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _vision = GetComponent<Vision>();
            _vision.EventPlayerSeeChanged += PlayerSawEvent;
        }
        private void OnDestroy()
        {
            _vision.EventPlayerSeeChanged -= PlayerSawEvent;

        }
        public void StartChase()
        {
            _isChasing = true;
        }

        public void CancellChase()
        {
            _isChasing = false;
        }
        public void UpdateChase()
        {
            if (IsInAttackRange())
            {
                _enemyController.StateMachine.ChangeState(_enemyController.AttackState);
            }
            else
            {
                Move();
            }
        }
        private void Move()
        {
            GetMoveDirection();
            _enemyController.Movement.Move(_direction, _enemyController.PatrolSpeed);
        }
        private void GetMoveDirection()
        {
            Vector2 direction = (Vector2)_targetPos - (Vector2)transform.position;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;

            E_FaceDirection enemyDirection = direction.x > 0 ? E_FaceDirection.right : E_FaceDirection.left;
            _enemyController.ChangeFaceDirection(enemyDirection);
        }
        private bool IsInAttackRange()
        {
            Vector2 distance = (Vector2)(_targetPos - transform.position);
            if (distance.magnitude < _NecesseryDistance)
                return true;
            return false;
        }
        private void PlayerSawEvent(Transform target)
        {
            if (_isChasing)
            {
                if (target == null)
                {
                    _enemyController.StateMachine.ChangeState(_enemyController.IdleState);
                    IDistractable distractable = GetComponent<IDistractable>();
                    if (distractable != null)
                    {
                        distractable.ThereIsDistraction(_targetPos, _playerLostWaitTime);
                    }                 
                }

            }
            if(target != null)
            {
                _targetPos = target.position;
            }
        }
    }
}

using NJN.Runtime.Controllers.Enemy;
using System;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Distractable : BaseComponent, IDistractable
    {
        EnemyController _enemyController;
        private Vector3 _distractionPosition;
        private float _distractionTime;
        private Vector2 _direction;
        private Action _endFunction;
        private float _closeEnough = 0.2f;
        private bool _isDistracted;

        private void Start()
        {
            _enemyController = GetComponent<EnemyController>();
        }
        public void ThereIsDistraction(Vector3 position, float distractionTime)
        {
            _distractionPosition = position;
            _distractionTime = distractionTime;
            if(!_isDistracted )
            {
                _enemyController.StateMachine.CurrentState.TryToDistract();
            }
            else
            {
                GetMoveDirection();
            }
        }

        public void StartDistraction(Action EndFunction)
        {
            _endFunction = EndFunction;
            GetMoveDirection();
            _isDistracted = true;
        }
        public void CancellDistraction()
        {
            _isDistracted = false;
        }
        public void UpdateLogic()
        {
            if (HasReachedDistractionPosition())
            {
                _distractionTime -= Time.deltaTime;
                if (_distractionTime <= 0)
                {
                    _endFunction();
                }
            }
            else
            {
                _enemyController.Movement.Move(_direction, _enemyController.PatrolSpeed);
            }
        }

        private void GetMoveDirection()
        {
            Vector2 direction = (Vector2)_distractionPosition - (Vector2)transform.position;
            _direction = direction.x > 0 ? Vector2.right : Vector2.left;

            _enemyController.ChangeFaceDirection(_direction.x);
        }

        private bool HasReachedDistractionPosition()
        {
            float distance = Mathf.Abs(_distractionPosition.x - transform.position.x);

            if (distance < _closeEnough)
                return true;

            return false;
        }
    }
}

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
        public Vector2 Direction { get; private set; }
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
        }
        
        public void UpdatePhysics()
        {
            if (!HasReachedDistractionPosition())
            {
                _enemyController.Movement.PhysicsHorizontalMove(Direction, false);
            }
        }

        private void GetMoveDirection()
        {
            Vector2 direction = (Vector2)_distractionPosition - (Vector2)transform.position;
            Direction = direction.x > 0 ? Vector2.right : Vector2.left;

            _enemyController.ChangeFaceDirection(Direction.x);
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

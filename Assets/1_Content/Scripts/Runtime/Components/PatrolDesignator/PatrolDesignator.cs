﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class PatrolDesignator : BaseComponent, IPatrolDesignator
    {
        [BoxGroup("Patrol Settings"), SerializeField]
        private List<Vector2> _patrolPoints = new();
        [BoxGroup("Patrol Settings"), SerializeField, Tooltip("How close to the patrol point to consider it reached")]
        private float _closeEnough = 0.1f;
        [BoxGroup("Patrol Settings"), SerializeField]
        private bool _showGizmos = true;
        
        private Vector3 _initialPosition;
        private int _patrolPointIndex = -1;

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _RayCheckDistance = 1;

        private Vector2 _direction;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        public Vector2 GetNextPointDirection(Vector2 position)
        {
            if (_patrolPoints.Count == 0)
            {
                Debug.Log("[PatrolDesignator] No patrol points set!");
                return Vector2.zero;
            }
            if (_patrolPointIndex == -1)
                ChangePatrolPoint();

            //ChangePatrolPoint();
            Vector2 worldTarget = GetWorldPatrolPoint(_patrolPointIndex);
            _direction = worldTarget - position;
            return _direction;
        }

        public bool HasArrivedAtPoint(Vector2 position)
        {
            if (_patrolPointIndex == -1)
            {
                return false;
            }
            bool shouldChangeDirection;
            if (IsPathBlocked())
                shouldChangeDirection = true;
            else
            {
                Vector2 worldTarget = GetWorldPatrolPoint(_patrolPointIndex);
                 shouldChangeDirection = Mathf.Abs(position.x - worldTarget.x) <= _closeEnough;
            }

            if(shouldChangeDirection)
            {
                ChangePatrolPoint();
            }
            return shouldChangeDirection;
        }

        private void ChangePatrolPoint()
        {
            _patrolPointIndex = (_patrolPointIndex + 1) % _patrolPoints.Count;
        }
        private bool IsPathBlocked()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _RayCheckDistance, _layerMask);
            if (hit.collider != null)
            {
                return true;
            }
            return false;
        }
        private Vector2 GetWorldPatrolPoint(int index)
        {
            return (Vector2)_initialPosition + _patrolPoints[index];
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_showGizmos || _patrolPoints.Count == 0) return;

            if (!Application.isPlaying) _initialPosition = transform.position;
            Gizmos.color = Color.yellow;
            foreach (Vector2 point in _patrolPoints)
            {
                Gizmos.DrawWireSphere(_initialPosition + (Vector3)point, 0.2f);
            }
        }
#endif
    }
}

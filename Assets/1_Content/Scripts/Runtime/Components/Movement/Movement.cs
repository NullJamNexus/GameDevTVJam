using System;
using NJN.Runtime.Controllers.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : BaseComponent, IMovement
    {
        [field: FoldoutGroup("Movement"), SerializeField]
        public float WalkSpeed { get; private set; } = 5f;
        [field: FoldoutGroup("Movement"), SerializeField]
        public float RunSpeed { get; private set; } = 8f;
        
        // TODO: Can make a climbing component if needed...
        public IClimbable Climbable { get; set; }
        
        private Rigidbody2D _rigidbody;

        [SerializeField] private bool _isEnemy;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _RayCheckDistance = 1;

        private Vector2 _direction;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("[Movement] Rigidbody2D is missing. Disabling component...");
                enabled = false;
            }
        }
        
        public void PhysicsMove(Vector2 direction, bool isSprinting, float? speed = null)
        {
            if (_isEnemy)
            {
                if (IsPathBlocked())
                    GetComponent<EnemyController>().SwitchToIdle();
            }
            float moveSpeed = speed ?? (isSprinting ? RunSpeed : WalkSpeed);
            _rigidbody.velocity = direction * moveSpeed;
        }
        
        public void PhysicsHorizontalMove(Vector2 direction, bool isSprinting, float? speed = null)
        {
            float moveSpeed = speed ?? (isSprinting ? RunSpeed : WalkSpeed);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction.x * moveSpeed;
            _rigidbody.velocity = velocity;
        }
        
        public void PhysicsVerticalMove(Vector2 direction, bool isSprinting, float? speed = null)
        {
            float moveSpeed = speed ?? (isSprinting ? RunSpeed : WalkSpeed);
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction.y * moveSpeed;
            _rigidbody.velocity = velocity;
        }
        
        public void PhysicsStop()
        {
            _rigidbody.velocity = Vector2.zero;
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
    }
}
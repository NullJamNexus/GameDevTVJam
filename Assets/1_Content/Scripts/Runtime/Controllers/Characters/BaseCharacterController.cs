using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class BaseCharacterController : BaseController<BaseCharacterController, CharacterState>, IDamagable
    {
        [field: FoldoutGroup("Physics"), SerializeField]
        public Rigidbody2D Rigidbody { get; private set; }
        [field: FoldoutGroup("Physics"), SerializeField]
        public Collider2D Collider { get; private set; }
        [field: FoldoutGroup("Physics"), SerializeField, ReadOnly]
        public bool IsGrounded { get; protected set; }
        [field: FoldoutGroup("Physics"), SerializeField, ReadOnly]
        public bool IsFacingRight { get; protected set; } = true;
        [field: FoldoutGroup("Physics"), SerializeField]
        private LayerMask _groundLayers;
        [field: FoldoutGroup("Physics"), SerializeField]
        private float _groundCheckDistance = 0.2f;
        [field: FoldoutGroup("Physics"), SerializeField]
        private bool _showDebugLine = false;
        
        public SpriteRenderer Model { get; private set; }
        public Animator Animator { get; private set; }
        public Transform Transform => transform;
        
        protected override void Awake()
        {
            base.Awake();
            
            Rigidbody ??= GetComponent<Rigidbody2D>();
            Collider ??= GetComponent<Collider2D>();
            Animator = GetComponentInChildren<Animator>();
            if (Animator == null)
            {
                Debug.LogError("[CharacterController] Animator not found in: " + name);
            }
            Model = GetComponentInChildren<SpriteRenderer>();
            if (Model == null)
            {
                Debug.LogError("[CharacterController] Model (SpriteRenderer) not found in: " + name);
            }
        }
        
        protected override void Update()
        {
            base.Update();
            
            CheckIfGrounded();
        }

        private void CheckIfGrounded()
        {
            Vector2 colliderBoundsBottomCenter = new (Collider.bounds.center.x, Collider.bounds.min.y);
            RaycastHit2D hit = Physics2D.Raycast(colliderBoundsBottomCenter, Vector2.down, _groundCheckDistance, _groundLayers);
            IsGrounded = hit.collider != null;
        }
        
        public void Flip()
        {
            IsFacingRight = !IsFacingRight;
            transform.localScale = new Vector3(IsFacingRight ? 1 : -1, 1, 1);
        }
        
        public void Flip(Vector2 direction)
        {
            IsFacingRight = direction.x > 0;
            transform.localScale = new Vector3(IsFacingRight ? 1 : -1, 1, 1);
        }

        public virtual void TakeDamage(float damage)
        {
            // No-Op
        } 

        private void OnCollisionEnter2D(Collision2D other) => StateMachine.CurrentState.OnCollisionEnter(other);
        private void OnCollisionStay2D(Collision2D other) => StateMachine.CurrentState.OnCollisionStay(other);
        private void OnCollisionExit2D(Collision2D other) => StateMachine.CurrentState.OnCollisionExit(other);
        private void OnTriggerEnter2D(Collider2D other) => StateMachine.CurrentState.OnTriggerEnter(other);
        private void OnTriggerStay2D(Collider2D other) => StateMachine.CurrentState.OnTriggerStay(other);
        private void OnTriggerExit2D(Collider2D other) => StateMachine.CurrentState.OnTriggerExit(other);

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (!_showDebugLine) return;
            
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Collider ??= GetComponent<Collider2D>();
            Vector2 colliderBoundsBottomCenter = new (Collider.bounds.center.x, Collider.bounds.min.y);
            Gizmos.DrawLine(colliderBoundsBottomCenter, colliderBoundsBottomCenter + Vector2.down * _groundCheckDistance);
        }
#endif
    }
}
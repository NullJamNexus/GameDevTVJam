using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class BaseCharacterController : BaseController<BaseCharacterController, CharacterState>, IDamagable
    {
        [field: BoxGroup("Stats"), SerializeField]
        public float MovementSpeed { get; private set; } = 5f;
        [field: BoxGroup("Stats"), SerializeField]
        public int BaseDamage { get; private set; } = 10;
        
        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            Animator = GetComponentInChildren<Animator>();
            if (Animator == null)
            {
                Debug.LogError("[CharacterController] Animator not found in: " + name);
            }
        }

        public virtual void TakeDamage(int damage)
        {
            // TODO: proper implementation
            Destroy(gameObject);
        } 

        private void OnCollisionEnter2D(Collision2D other) => StateMachine.CurrentState.OnCollisionEnter(other);
        private void OnCollisionStay2D(Collision2D other) => StateMachine.CurrentState.OnCollisionStay(other);
        private void OnCollisionExit2D(Collision2D other) => StateMachine.CurrentState.OnCollisionExit(other);
        private void OnTriggerEnter2D(Collider2D other) => StateMachine.CurrentState.OnTriggerEnter(other);
        private void OnTriggerStay2D(Collider2D other) => StateMachine.CurrentState.OnTriggerStay(other);
        private void OnTriggerExit2D(Collider2D other) => StateMachine.CurrentState.OnTriggerExit(other);
    }
}
using UnityEngine;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class CharacterController : BaseController<CharacterController, CharacterState>
    {
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
    }
}
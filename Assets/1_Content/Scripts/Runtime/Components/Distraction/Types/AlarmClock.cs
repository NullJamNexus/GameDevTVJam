using System;
using NJN.Runtime.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class AlarmClock : InteractableComponent, IDamagable
    {
        [BoxGroup("Settings"), SerializeField]
        private LayerMask _distractableLayers;
        [BoxGroup("Settings"), SerializeField]
        private float _distractionRange = 10f;
        [BoxGroup("Settings"), SerializeField]
        private float _durability = 20f;

        public Collider2D _collider2D;

        public Transform Transform => transform;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _collider2D.enabled = false;
        }

        [Button(ButtonSizes.Large)]
        public void TestDistract()
        {
            CauseDistraction();
            _collider2D.enabled = true;
        }

        private void CauseDistraction()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _distractionRange, _distractableLayers);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out IDistractable distractable))
                {
                    distractable.Distract(transform.position);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            _durability -= damage;
            if (_durability <= 0)
            {
                Destroy(gameObject);
            }
        }

        public override void Interact(IInteractor interactor)
        {
            CauseDistraction();
            _collider2D.enabled = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _distractionRange);
        }
#endif
    }
}
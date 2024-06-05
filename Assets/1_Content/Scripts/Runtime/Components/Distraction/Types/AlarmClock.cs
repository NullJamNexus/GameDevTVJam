using System;
using System.Collections.Generic;
using NJN.Runtime.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;

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
        [BoxGroup("Settings"), SerializeField, ValueDropdown(nameof(GetLayerNames))]
        private string _destroyableLayer;

        private Animator _animator;
        private const string _tvOnAnimName = "IsOn";

        public Collider2D _collider2D;

        public Transform Transform => transform;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            //_collider2D.enabled = false;
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
                Debug.LogError("Animator is missing on " + gameObject.name);
        }

        [Button(ButtonSizes.Large)]
        public void TestDistract()
        {
            CauseDistraction();
            _collider2D.enabled = true;
        }

        private void CauseDistraction()
        {
            _animator.SetBool(_tvOnAnimName, true);
            gameObject.layer = LayerMask.NameToLayer(_destroyableLayer);
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
            base.Interact(interactor);
            HideInteractPrompt();
            _collider2D.enabled = true;
            CauseDistraction();
        }

        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
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
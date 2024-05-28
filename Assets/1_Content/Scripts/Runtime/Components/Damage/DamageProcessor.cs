using NJN.Runtime.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class DamageProcessor : BaseComponent, IDamageProcessor
    {
        [BoxGroup("Settings"), SerializeField]
        private float _damage = 10f;
        [field: BoxGroup("Settings"), SerializeField]
        public float AttackRange { get; private set; } = 1f;

        public void DealDamage(IDamagable damagable, float damageAmount = -1f)
        {
            damagable.TakeDamage(Mathf.Approximately(damageAmount, -1f) ? _damage : damageAmount);
        }
    }
}
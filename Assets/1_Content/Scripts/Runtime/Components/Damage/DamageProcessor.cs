using NJN.Runtime.Controllers;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class DamageProcessor : BaseComponent, IDamageProcessor
    {
        [SerializeField]
        private float _damage = 10f;
        
        public void DealDamage(IDamagable damagable, float damageAmount = -1f)
        {
            damagable.TakeDamage(Mathf.Approximately(damageAmount, -1f) ? _damage : damageAmount);
        }
    }
}
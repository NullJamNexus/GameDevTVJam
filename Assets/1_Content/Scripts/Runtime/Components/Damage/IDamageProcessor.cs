using NJN.Runtime.Controllers;

namespace NJN.Runtime.Components
{
    public interface IDamageProcessor : IComponent
    {
        public float AttackRange { get; }
        public void DealDamage(IDamagable damagable, float damageAmount = -1f);
    }
}
using NJN.Runtime.Controllers;

namespace NJN.Runtime.Components
{
    public class DamageProcessor : BaseComponent, IDamageProcessor
    {
        public void DealDamage(IDamagable damagable, int damageAmount)
        {
            damagable.TakeDamage(damageAmount);
        }
    }
}
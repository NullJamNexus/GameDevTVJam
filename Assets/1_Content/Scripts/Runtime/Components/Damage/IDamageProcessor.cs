using NJN.Runtime.Controllers;

namespace NJN.Runtime.Components
{
    public interface IDamageProcessor : IComponent
    {
        public void DealDamage(IDamagable damagable, int damageAmount);
    }
}
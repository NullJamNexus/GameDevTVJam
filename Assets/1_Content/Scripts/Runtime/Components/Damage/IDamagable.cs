using UnityEngine;

namespace NJN.Runtime.Controllers
{
    public interface IDamagable
    {
        public Transform Transform { get; }
        public void TakeDamage(float damage);
    }
}
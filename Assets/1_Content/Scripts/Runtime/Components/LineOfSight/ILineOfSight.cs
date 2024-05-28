using NJN.Runtime.Controllers;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface ILineOfSight : IComponent
    {
        public float CheckInterval { get; }
        public IDamagable DamagableTarget { get; set; }
        public bool TryGetVisibleDamagable(Vector2 direction, out IDamagable damagable);
    }
}
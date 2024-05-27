using System;

namespace NJN.Runtime.Components
{
    public interface IHealth : IComponent
    {
        //public event Action<float, float> HealthChangedEvent;
        
        public Stat HealthStat { get; }
        public event Action DiedEvent;
        
        public void TakeDamage(float damage);
    }
}
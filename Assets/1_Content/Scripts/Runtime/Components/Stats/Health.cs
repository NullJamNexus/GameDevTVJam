using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Health : BaseComponent, IHealth
    {
        [FoldoutGroup("Health"), SerializeField]
        private bool _isInvincible;
        [field: FoldoutGroup("Health"), SerializeField, HideLabel]
        public Stat HealthStat { get; private set; }

        //public event Action<float, float> HealthChangedEvent;
        public event Action DiedEvent;
        
        protected virtual void Awake()
        {
            HealthStat.Reset();
        }
        
        public void TakeDamage(float amount)
        {
            if (_isInvincible) return;
            
            HealthStat.Current.Value = Math.Max(0, HealthStat.Current.Value - amount);
            //HealthChangedEvent?.Invoke(HealthStat.Current, HealthStat.Max);
            
            if (HealthStat.Current.Value <= 0)
            {
                DiedEvent?.Invoke();
            }
        }
        
        public void SetInvincibility(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
    }
}
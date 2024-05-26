using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Health : BaseComponent, IHealth
    {
        [FoldoutGroup("Health"), SerializeField]
        private bool _isInvincible;
        [FoldoutGroup("Health"), SerializeField, HideLabel]
        private Stat _healthStat;

        public event Action<float, float> HealthChangedEvent;
        public event Action DiedEvent;
        
        protected virtual void Awake()
        {
            _healthStat.Reset();
        }
        
        public void TakeDamage(float amount)
        {
            if (_isInvincible) return;
            
            _healthStat.Current = Math.Max(0, _healthStat.Current - amount);
            HealthChangedEvent?.Invoke(_healthStat.Current, _healthStat.Max);
            
            if (_healthStat.Current <= 0)
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
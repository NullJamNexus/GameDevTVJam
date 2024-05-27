using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class SurvivalStats : Health, ISurvivalStats
    {
        [FoldoutGroup("Hunger"), SerializeField, HideLabel]
        private Stat _hungerStat;
        [FoldoutGroup("Hunger"), SerializeField]
        private float _hungerLossRate;
        
        [FoldoutGroup("Thirst"), SerializeField, HideLabel]
        private Stat _thirstStat;
        [FoldoutGroup("Thirst"), SerializeField]
        private float _thirstLossRate;
        
        [FoldoutGroup("Damage Rate"), SerializeField]
        private float _damageRate;
        
        public event Action<float, float> FoodChangedEvent;
        public event Action<float, float> WaterChangedEvent;

        [Button(ButtonSizes.Large)]
        private void Reset()
        {
            _hungerStat.Reset();
            _thirstStat.Reset();
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            _hungerStat.Reset();
            _thirstStat.Reset();
        }
        
        private void Update()
        {
            StatLossOvertime(_hungerStat, _hungerLossRate, FoodChangedEvent);
            StatLossOvertime(_thirstStat, _thirstLossRate, WaterChangedEvent);
        }

        private void StatLossOvertime(Stat stat, float lossRate, Action<float, float> changedEvent)
        {
            if (stat.Current > 0)
            {
                stat.Current = Mathf.Max(0, stat.Current - lossRate * Time.deltaTime);
                changedEvent?.Invoke(stat.Current, stat.Max);
            }
            else
            {
                float damage = _damageRate * Time.deltaTime;
                TakeDamage(damage);
            }
        }

        public void AddFood(float amount)
        {
            _hungerStat.Current = Mathf.Min(_hungerStat.Current + amount, _hungerStat.Max);
        }
        
        public void AddWater(float amount)
        {
            _thirstStat.Current = Mathf.Min(_thirstStat.Current + amount, _thirstStat.Max);
        }
    }
}
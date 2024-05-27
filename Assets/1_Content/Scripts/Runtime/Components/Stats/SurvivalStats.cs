using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class SurvivalStats : Health, ISurvivalStats
    {
        [field: FoldoutGroup("Hunger"), SerializeField, HideLabel]
        public Stat HungerStat { get; private set; }
        [FoldoutGroup("Hunger"), SerializeField]
        private float _hungerLossRate;
        
        [field: FoldoutGroup("Thirst"), SerializeField, HideLabel]
        public Stat ThirstStat { get; private set; }
        [FoldoutGroup("Thirst"), SerializeField]
        private float _thirstLossRate;
        
        [FoldoutGroup("Damage Rate"), SerializeField]
        private float _damageRate;
        
        //public event Action<float, float> FoodChangedEvent;
        //public event Action<float, float> WaterChangedEvent;

        [Button(ButtonSizes.Large)]
        private void Reset()
        {
            HungerStat.Reset();
            ThirstStat.Reset();
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            HungerStat.Reset();
            ThirstStat.Reset();
        }
        
        private void Update()
        {
            StatLossOvertime(HungerStat, _hungerLossRate); //, FoodChangedEvent);
            StatLossOvertime(ThirstStat, _thirstLossRate); //, WaterChangedEvent);
        }

        private void StatLossOvertime(Stat stat, float lossRate) //, Action<float, float> changedEvent)
        {
            if (stat.Current.Value > 0)
            {
                stat.Current.Value = Mathf.Max(0, stat.Current.Value - lossRate * Time.deltaTime);
                //changedEvent?.Invoke(stat.Current, stat.Max);
            }
            else
            {
                float damage = _damageRate * Time.deltaTime;
                TakeDamage(damage);
            }
        }

        public void AddFood(float amount)
        {
            HungerStat.Current.Value = Mathf.Min(HungerStat.Current.Value + amount, HungerStat.Max.Value);
        }
        
        public void AddWater(float amount)
        {
            ThirstStat.Current.Value = Mathf.Min(ThirstStat.Current.Value + amount, ThirstStat.Max.Value);
        }
    }
}
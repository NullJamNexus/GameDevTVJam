using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class SurvivalStats : Health, ISurvivalStats
    {
        [FoldoutGroup("Food"), SerializeField, HideLabel]
        private Stat _foodStat;
        [FoldoutGroup("Food"), SerializeField]
        private float _foodLossRate;
        
        [FoldoutGroup("Water"), SerializeField, HideLabel]
        private Stat _waterStat;
        [FoldoutGroup("Water"), SerializeField]
        private float _waterLossRate;
        
        [FoldoutGroup("Damage Rate"), SerializeField]
        private float _damageRate;
        
        public event Action<float, float> FoodChangedEvent;
        public event Action<float, float> WaterChangedEvent;

        [Button(ButtonSizes.Large)]
        private void Reset()
        {
            _foodStat.Reset();
            _waterStat.Reset();
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            _foodStat.Reset();
            _waterStat.Reset();
        }
        
        private void Update()
        {
            StatLossOvertime(_foodStat, _foodLossRate, FoodChangedEvent);
            StatLossOvertime(_waterStat, _waterLossRate, WaterChangedEvent);
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
            _foodStat.Current = Mathf.Min(_foodStat.Current + amount, _foodStat.Max);
        }
        
        public void AddWater(float amount)
        {
            _waterStat.Current = Mathf.Min(_waterStat.Current + amount, _waterStat.Max);
        }
    }
}
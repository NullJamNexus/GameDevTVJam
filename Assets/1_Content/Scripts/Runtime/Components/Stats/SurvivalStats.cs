using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

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
        
        private SignalBus _signalBus;

        [Button(ButtonSizes.Large)]
        private void Reset()
        {
            HungerStat.Reset();
            ThirstStat.Reset();
        }
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            HungerStat.Reset();
            ThirstStat.Reset();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<CookedFoodSignal>(OnCookedFood);
            _signalBus.Subscribe<DrankWaterSignal>(OnDrankWater);
        }

        private void Update()
        {
            StatLossOvertime(HungerStat, _hungerLossRate);
            StatLossOvertime(ThirstStat, _thirstLossRate);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<CookedFoodSignal>(OnCookedFood);
            _signalBus.TryUnsubscribe<DrankWaterSignal>(OnDrankWater);
        }

        private void StatLossOvertime(Stat stat, float lossRate)
        {
            if (stat.Current.Value > 0)
            {
                stat.Current.Value = Mathf.Max(0, stat.Current.Value - lossRate * Time.deltaTime);
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
        
        private void OnCookedFood(CookedFoodSignal signal)
        {
            AddFood(signal.Amount);
        }
        
        private void OnDrankWater(DrankWaterSignal signal)
        {
            AddWater(signal.Amount);
        }
    }
}
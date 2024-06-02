using System;
using NJN.Runtime.Components;
using NJN.Runtime.Managers.Signals;
using NJN.Runtime.UI.Panels;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class LevelInventory : MonoBehaviour, ILevelInventory
    {
        [field: BoxGroup("Resources"), SerializeField, InlineProperty]
        public ReactiveProperty<int> Food { get; private set; }
        [field: BoxGroup("Resources"), SerializeField, InlineProperty]
        public ReactiveProperty<int> Fuel { get; private set; }
        [field: BoxGroup("Resources"), SerializeField]
        private float _fuelDepletionInterval = 10f;
        [field: BoxGroup("Resources"), SerializeField, InlineProperty]
        public ReactiveProperty<int> Scraps { get; private set; }
        
        private SignalBus _signalBus;
        private IDisposable _fuelDepletionDisposable;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<ResourceCollectedSignal>(OnResourceCollected);
            _signalBus.Subscribe<DestinationSelectedSignal>(OnDestinationSelected);
            StartFuelDepletion();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ResourceCollectedSignal>(OnResourceCollected);
            _signalBus.TryUnsubscribe<DestinationSelectedSignal>(OnDestinationSelected);
            StopFuelDepletion();
        }
        
        public void AddFood(int amount)
        {
            Food.Value += amount;
        }
        
        public void AddFuel(int amount)
        {
            Fuel.Value += amount;
        }
        
        public void AddScraps(int amount)
        {
            Scraps.Value += amount;
        }
        
        public void AddResources(int food, int fuel, int scraps)
        {
            AddFood(food);
            AddFuel(fuel);
            AddScraps(scraps);
        }
        
        private void OnResourceCollected(ResourceCollectedSignal signal)
        {
            AddResources(signal.FoodAmount, signal.FuelAmount, signal.ScrapsAmount);
        }
        
        private void OnDestinationSelected(DestinationSelectedSignal signal)
        {
            AddFuel(-signal.DestinationData.FuelCost);
        }
        
        private void StartFuelDepletion()
        {
            _fuelDepletionDisposable = Observable.Interval(TimeSpan.FromSeconds(_fuelDepletionInterval))
                .Subscribe(_ => DepleteFuel());
        }
        
        private void StopFuelDepletion()
        {
            _fuelDepletionDisposable?.Dispose();
        }
        
        private void DepleteFuel()
        {
            if (Fuel.Value > 0)
                Fuel.Value--;
            else
            {
                _signalBus.Fire(new FuelDepletedSignal());
                StopFuelDepletion();
                _signalBus.Fire(new GameLostSignal());
            }
        }
    }
}
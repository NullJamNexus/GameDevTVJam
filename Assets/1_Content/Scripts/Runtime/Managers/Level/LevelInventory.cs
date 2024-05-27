using System;
using NJN.Runtime.Components;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace NJN.Runtime.Managers
{
    [Serializable]
    public class LevelInventory
    {
        [field: SerializeField, ReadOnly, InlineProperty]
        public ReactiveProperty<int> Food { get; private set; }
        [field: SerializeField, ReadOnly, InlineProperty]
        public ReactiveProperty<int> Fuel { get; private set; }
        [field: SerializeField, ReadOnly, InlineProperty]
        public ReactiveProperty<int> Scraps { get; private set; }
        
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
    }
}
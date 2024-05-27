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
        
        public void AddResource(Resource resource)
        {
            if (resource is FoodResource foodResource)
            {
                AddFuel(foodResource.Amount);
            }
            else if (resource is FuelResource fuelResource)
            {
                AddFuel(fuelResource.Amount);
            }
            else if (resource is ScrapResource scrapResource)
            {
                AddScraps(scrapResource.Amount);
            }
        }
    }
}
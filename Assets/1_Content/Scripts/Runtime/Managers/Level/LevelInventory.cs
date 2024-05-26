using System;
using NJN.Runtime.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Managers
{
    [Serializable]
    public class LevelInventory
    {
        [field: SerializeField, ReadOnly]
        public float Fuel { get; private set; }
        [field: SerializeField, ReadOnly]
        public int Scraps { get; private set; }
        
        public void AddFuel(float amount)
        {
            Fuel += amount;
        }
        
        public void AddScraps(int amount)
        {
            Scraps += amount;
        }
        
        public void AddResource(Resource resource)
        {
            if (resource is FuelResource fuelResource)
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
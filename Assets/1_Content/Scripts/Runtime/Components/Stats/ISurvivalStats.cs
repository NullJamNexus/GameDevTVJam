using System;

namespace NJN.Runtime.Components
{
    public interface ISurvivalStats : IHealth
    {
        public event Action<float, float> FoodChangedEvent;
        public event Action<float, float> WaterChangedEvent;
        
        public void AddFood(float amount);
        public void AddWater(float amount);
    }
}
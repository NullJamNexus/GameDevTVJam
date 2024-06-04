namespace NJN.Runtime.Components
{
    public struct ResourceCollectedSignal
    {
        public int FoodAmount { get; }
        public int FuelAmount { get; }
        public int ScrapsAmount { get; }
        
        public ResourceCollectedSignal(int foodAmount, int fuelAmount, int scrapsAmount)
        {
            FoodAmount = foodAmount;
            FuelAmount = fuelAmount;
            ScrapsAmount = scrapsAmount;
        }
    }
}
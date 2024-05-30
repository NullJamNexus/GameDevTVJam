using UniRx;

namespace NJN.Runtime.Managers
{
    public interface ILevelInventory
    {
        public ReactiveProperty<int> Food { get; }
        public ReactiveProperty<int> Fuel { get; }
        public ReactiveProperty<int> Scraps { get; }
        
        public void AddFood(int amount);
        public void AddFuel(int amount);
        public void AddScraps(int amount);
        public void AddResources(int food, int fuel, int scraps);
    }
}
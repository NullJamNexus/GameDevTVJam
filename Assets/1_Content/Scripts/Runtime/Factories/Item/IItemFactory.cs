using NJN.Runtime.Components;

namespace NJN.Runtime.Factories
{
    public interface IItemFactory
    {
        public Resource CreateRandomResource();
    }
}
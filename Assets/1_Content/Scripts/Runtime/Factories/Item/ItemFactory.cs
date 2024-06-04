using NJN.Runtime.Components;
using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.Factories
{
    public class ItemFactory : IItemFactory, IInitializable
    {
        private readonly DiContainer _container;
        private Resource[] _resources;
        
        public ItemFactory(DiContainer container)
        {
            _container = container;
        }
        
        public void Initialize()
        {
            _resources = Resources.LoadAll<Resource>("ResourceItems");
        }
        
        public Resource CreateRandomResource()
        {
            int randomIndex = Random.Range(0, _resources.Length);
            Resource resourcePrefab = _resources[randomIndex];
            Resource resourceInstance = _container.InstantiatePrefabForComponent<Resource>(resourcePrefab);
            return resourceInstance;
        }
    }
}
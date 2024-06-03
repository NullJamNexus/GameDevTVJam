using System.Collections.Generic;
using System.Linq;
using NJN.Scriptables;
using NJN.Runtime.Controllers.Destination;
using NJN.Runtime.UI.Panels;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Factories
{
    public class DestinationsFactory : IDestinationsFactory, IInitializable
    {
        private readonly DiContainer _container;
        private List<DestinationOptionSO> _destinations;
        
        public DestinationsFactory(DiContainer container)
        {
            _container = container;
        }
        
        public void Initialize()
        {
            _destinations = Resources.LoadAll<DestinationOptionSO>("DestinationDatas").ToList();
        }
        
        public DestinationOptionVisual CreateRandomOption(GameObject parentObject, DestinationOptionVisual prefab)
        {
            if (_destinations.Count == 0)
            {
                Debug.LogError("No more destinations available.");
                return null;
            }
            
            int randomIndex = Random.Range(0, _destinations.Count);
            DestinationOptionSO destinationData = _destinations[randomIndex];
            RemoveDestinationOption(destinationData);
            DestinationOptionVisual destinationOptionVisual = _container.InstantiatePrefabForComponent<DestinationOptionVisual>(prefab, parentObject.transform);
            destinationOptionVisual.SetUpFromFactory(destinationData);
            return destinationOptionVisual;
        }
        
        private void RemoveDestinationOption(DestinationOptionSO destinationData)
        {
            _destinations.Remove(destinationData);
        }

        public DestinationController CreateDestination(DestinationOptionSO destinationData)
        {
            DestinationController destinationController = _container.InstantiatePrefabForComponent<DestinationController>(destinationData.DestinationPrefab);
            return destinationController;
        }
    }
}
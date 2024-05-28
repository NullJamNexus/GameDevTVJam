using NJN.Scriptables;
using NJN.Runtime.Controllers.Destination;
using NJN.Runtime.UI.Panels;
using UnityEngine;

namespace NJN.Runtime.Factories
{
    public interface IDestinationsFactory
    {
        public DestinationOptionVisual CreateRandomOption(GameObject parentObject, DestinationOptionVisual prefab);
        public void RemoveDestinationOption(DestinationOptionSO destinationData);
        public DestinationController CreateDestination(DestinationOptionSO destinationData);
    }
}
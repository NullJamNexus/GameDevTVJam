using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Scriptables
{
    [CreateAssetMenu(fileName = "DestinationOption", menuName = "NJN/Destinations/New DestinationOption")]
    public class DestinationOptionSO : ScriptableObject
    {
        [field: BoxGroup("Destination Option"), SerializeField]
        public string DestinationName { get; private set; }
        [field: BoxGroup("Destination Option"), SerializeField]
        public GameObject DestinationPrefab { get; private set; }
        [field: BoxGroup("Destination Option"), SerializeField]
        public int FuelCost { get; private set; }
        [field: BoxGroup("Destination Option"), SerializeField, TextArea]
        public string DestinationInformation { get; private set; }
    }
}
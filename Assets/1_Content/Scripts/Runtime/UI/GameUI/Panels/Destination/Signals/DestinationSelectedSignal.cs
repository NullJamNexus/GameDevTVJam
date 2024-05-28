using NJN.Scriptables;

namespace NJN.Runtime.UI.Panels
{
    public struct DestinationSelectedSignal
    {
        public DestinationOptionSO DestinationData { get; }
        
        public DestinationSelectedSignal(DestinationOptionSO destinationData)
        {
            DestinationData = destinationData;
        }
    }
}
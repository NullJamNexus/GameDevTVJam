namespace NJN.Runtime.Managers.Level.Signals
{
    public struct DestinationTransitionFinishedSignal
    {
        public float DecelerationRate { get; }
        
        public DestinationTransitionFinishedSignal(float decelerationRate)
        {
            DecelerationRate = decelerationRate;
        }
    }
}
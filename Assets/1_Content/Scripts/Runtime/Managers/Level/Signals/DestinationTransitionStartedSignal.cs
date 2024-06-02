namespace NJN.Runtime.Managers.Level.Signals
{
    public struct DestinationTransitionStartedSignal
    {
        public float MaxSpeed { get; }
        public float AccelerationRate { get; }
        
        public DestinationTransitionStartedSignal(float maxSpeed, float accelerationRate)
        {
            MaxSpeed = maxSpeed;
            AccelerationRate = accelerationRate;
        }
    }
}
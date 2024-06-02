namespace NJN.Runtime.Managers.Level.Signals
{
    public struct ProgressUpdatedSignal
    {
        public int CurrentDestination { get; }
        public int TotalDestinations { get; }
        
        public ProgressUpdatedSignal(int currentDestination, int totalDestinations)
        {
            CurrentDestination = currentDestination;
            TotalDestinations = totalDestinations;
        }
    }
}
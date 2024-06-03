namespace NJN.Runtime.Managers.Level.Signals
{
    public enum GameOverReason
    {
        NoHealth,
        EmptyFuel,
        ReachedBunker
    }

    public struct GameOverSignal
    {
        public GameOverReason Reason { get; }

        public GameOverSignal(GameOverReason reason)
        {
            Reason = reason;
        }                
    }
}
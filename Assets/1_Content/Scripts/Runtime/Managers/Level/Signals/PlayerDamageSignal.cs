namespace NJN.Runtime.Managers
{
    public struct PlayerDamageSignal
    {
        public float Amount { get; private set; }
        
        public PlayerDamageSignal(float amount)
        {
            Amount = amount;
        }
    }
}
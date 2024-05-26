namespace NJN.Runtime.Components
{
    public struct ResourceCollectedSignal
    {
        public Resource Resource { get; private set; }
        
        public ResourceCollectedSignal(Resource resource)
        {
            Resource = resource;
        }
    }
}
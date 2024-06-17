namespace NJN.Runtime.Components
{
    public interface IRemoteInteractor : IComponent
    {
        public IRemoteInteractableComponent RemoteInteractable { get; }

        public IRemoteInteractableComponent PlacedRemoteInteractable { get; }
        public void RemoteInteract();
    }
}

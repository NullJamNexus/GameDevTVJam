namespace NJN.Runtime.Components
{
    public interface IRemoteInteractableComponent
    {
        public void RemoteInteract();

        public void SelectedForRemoteInteraction();
        public void ShowRemoteInteractPrompt();
        public void HideRemoteInteractPrompt();
    }
}

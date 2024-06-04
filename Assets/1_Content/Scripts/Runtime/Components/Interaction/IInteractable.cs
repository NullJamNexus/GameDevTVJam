using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Components
{
    public interface IInteractable
    {
        public void Interact(IInteractor interactor);
        public void ShowInteractPrompt();
        public void HideInteractPrompt();
    }
}
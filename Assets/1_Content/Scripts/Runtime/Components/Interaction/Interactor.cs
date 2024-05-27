namespace NJN.Runtime.Components
{
    public class Interactor : BaseComponent, IInteractor
    {
        public IInteractable Interactable { get; set; }
    }
}
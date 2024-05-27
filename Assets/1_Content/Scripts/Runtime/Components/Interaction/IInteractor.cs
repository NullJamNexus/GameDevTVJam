namespace NJN.Runtime.Components
{
    public interface IInteractor : IComponent
    {
        public IInteractable Interactable { get; set; }
    }
}
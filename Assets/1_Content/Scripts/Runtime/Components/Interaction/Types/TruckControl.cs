using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Components
{
    public class TruckControl : InteractableComponent
    {
        public override void Interact(IInteractor interactor)
        {
            base.Interact(interactor);
            
            _signalBus.Fire(new PickDestinationSignal());
        }
    }
}
using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Components
{
    public class TruckControl : InteractableComponent
    {
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            _signalBus.Fire(new PickDestinationSignal());
        }
    }
}
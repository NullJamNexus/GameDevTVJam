


using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Managers.Level.Signals;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class TruckControl : InteractableComponent
    {
        public bool _inTransition;
        private SignalBus _signalBusB;
        public override void Interact(IInteractor interactor)
        {
            base.Interact(interactor);
            Debug.Log(_inTransition);
            if(!_inTransition)
            {
                
                
                _signalBus.Fire(new PickDestinationSignal());
            }

        }




    }
}
using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class HideSpot : HideSpotInteractableComponent
    {
        public override void Interact(IInteractor interactor)
        {
            base.Interact(interactor);

            IsHiding = !IsHiding;
            ShowInteractPrompt();

            if(IsHiding)
            {
                _signalBus.Fire(new PlayerHideSignal());
            }
            else
            {
                _signalBus.Fire(new PlayerUnhideSignal());
            }
        }
    }
}
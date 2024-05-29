using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class HideSpot : HideSpotInteractableComponent
    {
        [BoxGroup("Hide Style"), SerializeField]
        private EHideStyle _hideStyle;

        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            IsHiding = !IsHiding;
            ShowInteractPrompt();

            if(IsHiding)
            {
                _signalBus.Fire(new HideSignal(_hideStyle));
            }
            else
            {
                _signalBus.Fire(new EndHideSignal());
            }
        }
    }
}
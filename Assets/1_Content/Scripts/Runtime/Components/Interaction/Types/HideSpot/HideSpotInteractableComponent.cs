using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class HideSpotInteractableComponent : BaseComponent, IInteractable
    {
        [BoxGroup("Interaction"), SerializeField]
        protected string _hideText = "Hide [E]";

        [BoxGroup("Interaction"), SerializeField]
        protected string _endHideText = "End hide [E]";

        [BoxGroup("Interaction"), SerializeField]
        protected Vector2 _promptOffset = new(0f, 1f);

        protected bool IsHiding { get; set; }

        protected InteractionPrompt _interactionPrompt;
        protected SignalBus _signalBus;

        [Inject]
        private void Construct(InteractionPrompt interactionPrompt, SignalBus signalBus)
        {
            _interactionPrompt = interactionPrompt;
            _signalBus = signalBus;
        }

        public virtual void Interact(PlayerController player)
        {
        }
        public void ShowInteractPrompt()
        {
            Vector2 position = transform.position + (Vector3)_promptOffset;
            if (IsHiding)
            {

                string prompt = _endHideText;
                _interactionPrompt.ShowPrompt(prompt, position);
            }
            else
            {
                string prompt = _hideText;
                _interactionPrompt.ShowPrompt(prompt, position);
            }
        }

        public void HideInteractPrompt()
        {
            if (_interactionPrompt != null)
                _interactionPrompt.HidePrompt();
        }
    }
}
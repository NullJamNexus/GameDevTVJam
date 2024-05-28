using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public abstract class InteractableComponent : BaseComponent, IInteractable
    {
        [BoxGroup("Interaction"), SerializeField]
        protected string _promptText = "Press [E]";

        [BoxGroup("Interaction"), SerializeField]
        protected Vector2 _promptOffset = new(0f, 1f);

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
            // No-op
        }

        public virtual void ShowInteractPrompt()
        {
            Vector2 position = transform.position + (Vector3)_promptOffset;
            _interactionPrompt.ShowPrompt(_promptText, position);
        }

        public virtual void HideInteractPrompt()
        {
            if (_interactionPrompt != null)
                _interactionPrompt.HidePrompt();
        }
    }
}
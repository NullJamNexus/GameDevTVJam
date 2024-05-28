using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class DoorInteractableComponent : BaseComponent, IInteractable
    {
        [BoxGroup("Interaction"), SerializeField]
        protected string _openPromptText = "Open [E]";

        [BoxGroup("Interaction"), SerializeField]
        protected string _closePromptText = "Close [E]";

        [BoxGroup("Interaction"), SerializeField]
        protected Vector2 _promptOffset = new(0f, 1f);

        public bool IsOpen { get; protected set; }

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
            if (IsOpen)
            {
                
                string prompt =_closePromptText;
                _interactionPrompt.ShowPrompt(prompt, position);
            }
            else
            {
                string prompt = _openPromptText;
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

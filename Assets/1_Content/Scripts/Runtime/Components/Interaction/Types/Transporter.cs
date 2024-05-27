using System;
using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class Transporter : BaseComponent, IInteractable
    {
        [BoxGroup("Teleport Settings"), SerializeField]
        private Transform _target;
        [BoxGroup("Teleport Settings"), SerializeField]
        private Vector2 _offset;

        [BoxGroup("Interaction"), SerializeField]
        private string _promptText = "Press [E]";
        [BoxGroup("Interaction"), SerializeField]
        private Vector2 _promptOffset;
        
        private InteractionPrompt _interactionPrompt;
        
        [Inject]
        private void Construct(InteractionPrompt interactionPrompt)
        {
            _interactionPrompt = interactionPrompt;
        }
        
        public void Interact(PlayerController player)
        {
            if (_target == null) return;

            player.transform.position = TransportLocation();
        }

        public void ShowInteractPrompt()
        {
            Vector2 position = transform.position + (Vector3)_promptOffset;
            _interactionPrompt.ShowPrompt(_promptText, position);
        }

        public void HideInteractPrompt()
        {
            _interactionPrompt.HidePrompt();
        }

        private Vector2 TransportLocation()
        {
            return (Vector2)_target.position + _offset;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_target == null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _target.position);
            Gizmos.DrawWireSphere(TransportLocation(), 0.2f);
        }
#endif
    }
}
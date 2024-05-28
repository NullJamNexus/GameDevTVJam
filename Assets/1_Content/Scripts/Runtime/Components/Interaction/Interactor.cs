using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;

namespace NJN.Runtime.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactor : BaseComponent, IInteractor
    {
        [BoxGroup("Settings"), SerializeField]
        private LayerMask _interactableLayer;
        
        public IInteractable Interactable { get; set; }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Tools.IsInLayerMask(other.gameObject, _interactableLayer) 
                && other.TryGetComponent(out IInteractable interactable))
            {
                Interactable = interactable;
                Interactable.ShowInteractPrompt();
            }
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if (Tools.IsInLayerMask(other.gameObject, _interactableLayer) 
                && other.TryGetComponent(out IInteractable interactable) && Interactable == interactable)
            {
                Interactable = null;
                interactable.HideInteractPrompt();
            }
        }
    }
}
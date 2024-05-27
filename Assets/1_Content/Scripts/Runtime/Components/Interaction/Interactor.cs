using System;
using UnityEngine;

namespace NJN.Runtime.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactor : BaseComponent, IInteractor
    {
        public IInteractable Interactable { get; set; }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                Interactable = interactable;
                Interactable.ShowInteractPrompt();
            }
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && Interactable == interactable)
            {
                Interactable = null;
                interactable.HideInteractPrompt();
            }
        }
    }
}
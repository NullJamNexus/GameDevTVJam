using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Door : DoorInteractableComponent
    {
        [BoxGroup("Start State"), SerializeField]
        private bool _StartOpen;
        
        [BoxGroup("Components"), SerializeField]     
        private Collider2D _collider;


        private void Start()
        {
            IsOpen = _StartOpen;
            if (IsOpen)
                OpenDoorNoAnim();
        }

        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            IsOpen = !IsOpen;
            ShowInteractPrompt();

            if (IsOpen)
                OpenDoor();
            else
                CloseDoor();
        }
        protected virtual void OpenDoor()
        {
            _collider.enabled = false;
            //play open anim
        }
        protected virtual void CloseDoor()
        {
            _collider.enabled = true;
            // play close anim
        }

        protected virtual void OpenDoorNoAnim()
        {
            _collider.enabled = false;
            // change sprite to open
        }
    }
}

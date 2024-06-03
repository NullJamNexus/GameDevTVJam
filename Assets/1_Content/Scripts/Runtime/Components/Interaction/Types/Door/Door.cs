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

        [SerializeField]
        private Sprite _doorOpenSprite;

        [SerializeField]
        private Sprite _doorClosedSprite;

        private SpriteRenderer _spriteRenderer;


        private void Start()
        {
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            IsOpen = _StartOpen;
            if (IsOpen)
                OpenDoorNoAnim();
        }

        public override void Interact(IInteractor interactor)
        {
            base.Interact(interactor);

            IsOpen = !IsOpen;
            ShowInteractPrompt();

            if (IsOpen)
                OpenDoor();
            else
                CloseDoor();
        }
        protected virtual void OpenDoor()
        {
            _spriteRenderer.sprite = _doorOpenSprite;
            _signalBus.Fire(new DoorOpenSignal());
            _collider.enabled = false;
            //play open anim
        }
        protected virtual void CloseDoor()
        {
            _spriteRenderer.sprite = _doorClosedSprite;
            _signalBus.Fire(new DoorCloseSignal());
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

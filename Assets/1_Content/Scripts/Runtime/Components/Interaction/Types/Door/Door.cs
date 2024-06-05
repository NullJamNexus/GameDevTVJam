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

        private Animator _animator;
        private const string _openBoolAnimName = "IsOpen";

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
                Debug.LogError("Animator is missing on " + gameObject.name);
        }

        private void Start()
        {
            IsOpen = _StartOpen;
            if (IsOpen)
                OpenDoor();
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
            _signalBus.Fire(new DoorOpenSignal());
            _collider.enabled = false;
            _animator.SetBool(_openBoolAnimName, true);
        }
        protected virtual void CloseDoor()
        {
            _signalBus.Fire(new DoorCloseSignal());
            _collider.enabled = true;
            _animator.SetBool(_openBoolAnimName, false);
        }
    }
}

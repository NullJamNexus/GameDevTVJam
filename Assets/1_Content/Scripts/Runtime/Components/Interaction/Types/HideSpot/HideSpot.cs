using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class HideSpot : HideSpotInteractableComponent
    {
        [BoxGroup("Temp Options"), SerializeField]
        private Sprite _defaultSprite;
        [BoxGroup("Temp Options"), SerializeField]
        private Sprite _hideSprite;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer == null)
                Debug.LogError("SpriteRenderer is missing on " + gameObject.name);
        }

        public override void Interact(IInteractor interactor)
        {
            base.Interact(interactor);

            IsHiding = !IsHiding;
            ShowInteractPrompt();

            if(IsHiding)
            {
                _signalBus.Fire(new PlayerHideSignal());
                _spriteRenderer.sprite = _hideSprite;
            }
            else
            {
                _signalBus.Fire(new PlayerUnhideSignal());
                _spriteRenderer.sprite = _defaultSprite;
            }
        }
    }
}
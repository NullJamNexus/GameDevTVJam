using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.Components
{
    public enum EHideStyle { transparent, underObject }
    public class Hide : BaseComponent
    {
        [BoxGroup("Sprite"), SerializeField]
        private SpriteRenderer _spriteRenderer;
        [BoxGroup("Sprite"), SerializeField]
        private float _hideAlpha;

        [BoxGroup("HidenLayer"), SerializeField, ValueDropdown("GetLayerNames")]
        private string _HidenLayer;
        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }

        private Color _startColorOfSprite;
        private PlayerController _playerController;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private string _startLayer;

        protected SignalBus _signalBus;

        [Inject]
        private void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void OnEnable()
        {
            _signalBus.Subscribe<HideSignal>(HidePlayer);
            _signalBus.Subscribe<EndHideSignal>(EndHide);
        }
        private void Start()
        {
            _startColorOfSprite = _spriteRenderer.color;
            _playerController = GetComponent<PlayerController>();
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _startLayer = LayerMask.LayerToName(gameObject.layer);
        }
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<HideSignal>(HidePlayer);
            _signalBus.TryUnsubscribe<EndHideSignal>(EndHide);
        }
        private void EndHide()
        {
            //_playerController.StateMachine.ChangeState(_playerController.IdleState);

            _collider.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer(_startLayer);
            _rb.isKinematic = false;

            EndHideVisual();
        }

        private void HidePlayer(HideSignal signal)
        {
            //_playerController.StateMachine.ChangeState(_playerController.BusyState);

            _collider.isTrigger = true;
            _rb.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer(_HidenLayer);

            HideVisual(signal.Style);
        }

        private void HideVisual(EHideStyle hideStyle)
        {
            if(hideStyle == EHideStyle.transparent)
            {
                Color color = _startColorOfSprite;
                color.a = _hideAlpha;
                _spriteRenderer.color = color;
            }
            else if(hideStyle == EHideStyle.underObject)
            {
                print(" UnderObject Hide style visuals not implemented");
                // play anim
            }
        }

        private void EndHideVisual()
        {
            _spriteRenderer.color = _startColorOfSprite;
        }
    }
}
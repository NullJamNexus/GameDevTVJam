using NJN.Runtime.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NJN.Runtime.UI.StartMenu
{
    public class MenuHoverOn : BaseComponent, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _hoverSize = 1.2f;

        private Vector3 _originalScale;

        void Start()
        {
            _originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
                transform.localScale = _originalScale * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit();
        }

        public void PointerExit()
        {
            transform.localScale = _originalScale;
        }
    }
}

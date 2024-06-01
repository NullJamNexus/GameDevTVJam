using NJN.Runtime.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NJN.Runtime.UI.StartMenu
{
    public class MenuHoverOn : BaseComponent, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _hoverSize = 1.2f;

        private Vector3 _originalScale;

        Button button;

        void Start()
        {
            _originalScale = transform.localScale;
            button = GetComponent<Button>();
            button.onClick.AddListener(PointerExit);
        }
        private void OnDestroy()
        {
            button.onClick.RemoveListener(PointerExit); 
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
                transform.localScale = _originalScale * _hoverSize;
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

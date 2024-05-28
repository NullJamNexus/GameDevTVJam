using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.UI.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePanel : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;
        protected GameUI _gameUI;
        protected SignalBus _signalBus;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public virtual void SetUpPanel(GameUI gameUI, SignalBus signalBus)
        {
            _gameUI = gameUI;
            _signalBus = signalBus;
        }
        
        public virtual void DisposePanel()
        {
            // No-op
        }

        public virtual void TogglePanel(bool show)
        {
            Tools.ToggleVisibility(_canvasGroup, show);
        }
    }
}
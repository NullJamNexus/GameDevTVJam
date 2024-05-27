using NJN.Runtime.Components;
using NJN.Runtime.Input;
using NJN.Runtime.UI.Panels;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.UI
{
    public class GameUI : MonoBehaviour
    {
        private BasePanel[] _basePanels;

        private SignalBus _signalBus;
        private IInputProvider _inputProvider;
        
        [Inject]
        private void Construct(SignalBus signalBus, IInputProvider inputProvider)
        {
            _signalBus = signalBus;
            _inputProvider = inputProvider;
        }

        private void Awake()
        {
            _basePanels = GetComponentsInChildren<BasePanel>();
        }

        private void Start()
        {
            foreach (BasePanel panel in _basePanels)
            {
                panel.SetUpPanel(this, _signalBus);
            }
        }

        private void OnDestroy()
        {
            foreach (BasePanel panel in _basePanels)
            {
                panel.DisposePanel();
            }
        }

        public void TogglePanel(BasePanel panel, bool show)
        {
            if (show)
                _inputProvider.EnableUIControls();
            else
                _inputProvider.EnablePlayerControls();
            
            panel.TogglePanel(show);
        }
    }
}
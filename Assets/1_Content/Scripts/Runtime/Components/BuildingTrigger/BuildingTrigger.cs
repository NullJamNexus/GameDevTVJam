using NJN.Runtime.Managers.Level.Signals;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class BuildingTrigger : BaseComponent
    {
        private bool _isTransitionActive;
        private bool _isActive; // spawning building in 0,0 and directly interacting with the player
        private SignalBus _signalBus;
  
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DestinationTransitionStartedSignal>(TransitionStart);
            _signalBus.Subscribe<DestinationTransitionFinishedSignal>(TransitionEnd);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<DestinationTransitionStartedSignal>(TransitionStart);
            _signalBus.TryUnsubscribe<DestinationTransitionFinishedSignal>(TransitionEnd);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isTransitionActive && _isActive) 
            {
                _signalBus.Fire(new EnterBuildingSignal());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_isTransitionActive && _isActive)
            {
                _signalBus.Fire(new ExitBuildingSignal());
            }
        }
        private void ActivateBuilding()
        {
            _isActive = true;
        }

        private void TransitionStart()
        {
            _isTransitionActive = true;
        }

        private void TransitionEnd()
        {
            _isTransitionActive = false;
        }
    }
}
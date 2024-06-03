﻿using NJN.Runtime.Components;
using UnityEngine;
using Zenject;
using NJN.Runtime.SoundSignal;
using NJN.Runtime.Managers.Level.Signals;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class TruckController : MonoBehaviour
    {
        private TruckControl _truckControl;
        [SerializeField]
        private LayerMask _playerLayer;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<DestinationTransitionStartedSignal>(ChangeTransitionState);
            _signalBus.Subscribe<DestinationTransitionFinishedSignal>(ChangeTransitionState);
            _truckControl = GetComponentInChildren<TruckControl>();
        }

        private void ChangeTransitionState()
        {
            if(_truckControl._inTransition==true)
            {
                _truckControl._inTransition=false;
            }
            else
            {
                _truckControl._inTransition=true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus?.Fire(new EnteredTruckSignal());
                _signalBus?.Fire(new MusicSignal(EMusic.truck));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus?.TryFire(new ExitedTruckSignal());
                _signalBus?.Fire(new MusicSignal(EMusic.stop));
            }
        }
    }
}

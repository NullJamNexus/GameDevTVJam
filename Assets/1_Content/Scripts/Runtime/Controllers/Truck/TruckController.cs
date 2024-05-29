using System;
using NJN.Runtime.Components;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class TruckController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _playerLayer;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus.Fire(new EnteredTruckSignal());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus.Fire(new ExitedTruckSignal());
            }
        }
    }
}
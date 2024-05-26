using System;
using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public abstract class Resource : BaseComponent, IComponent, ICollectable
    {
        [field: BoxGroup("General"), SerializeField]
        public int Amount { get; private set; }
        
        protected SignalBus _signalBus;
      
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public abstract void Collect(PlayerController player);
    }
}
using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class WaterCooler : InteractableComponent
    {
        [BoxGroup("Water Cooler"), SerializeField]
        private float _waterToAdd = 100f;
        
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            _signalBus.Fire(new DrankWaterSignal(_waterToAdd));
        }
    }
}
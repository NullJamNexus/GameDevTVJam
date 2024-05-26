using NJN.Runtime.Controllers.Player;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class FuelResource : Resource
    {
        public override void Collect(PlayerController player)
        {
            _signalBus.Fire(new ResourceCollectedSignal(this));
            Destroy(gameObject);
        }
    }
}
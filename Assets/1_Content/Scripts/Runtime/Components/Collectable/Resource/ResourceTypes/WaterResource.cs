using NJN.Runtime.Controllers.Player;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class WaterResource : Resource
    {
        public override void Collect(PlayerController player)
        {
            player.Stats.AddWater(Amount);
            Destroy(gameObject);
        }
    }
}
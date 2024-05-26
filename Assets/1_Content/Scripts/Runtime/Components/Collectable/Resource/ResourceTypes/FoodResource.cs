using NJN.Runtime.Controllers.Player;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class FoodResource : Resource
    {
        public override void Collect(PlayerController player)
        {
            player.Stats.AddFood(Amount);
            Destroy(gameObject);
        }
    }
}
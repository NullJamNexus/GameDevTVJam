using NJN.Runtime.Controllers.Player;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class WaterResource : Resource
    {
        public override void Collect(ISurvivalStats stats)
        {
            stats.AddWater(Amount);
            Destroy(gameObject);
        }
    }
}
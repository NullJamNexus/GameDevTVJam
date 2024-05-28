﻿using NJN.Runtime.Controllers.Player;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class FuelResource : Resource
    {
        public override void Collect(ISurvivalStats stats)
        {
            _signalBus.Fire(new ResourceCollectedSignal(0, Amount, 0));
            Destroy(gameObject);
        }
    }
}
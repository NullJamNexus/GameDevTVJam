using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField]
        private float Starting { get; set; }
        [field: SerializeField, ReadOnly]
        public float Current { get; set; }
        [field: SerializeField, ReadOnly]
        public float Max { get; set; }
        
        public void Reset()
        {
            Current = Starting;
            Max = Starting;
        }
    }
}
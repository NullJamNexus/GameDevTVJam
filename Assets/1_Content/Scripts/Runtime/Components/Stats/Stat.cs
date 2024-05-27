using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace NJN.Runtime.Components
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField]
        private float Starting { get; set; } = 100f;
        [field: SerializeField, InlineProperty, ReadOnly]
        public ReactiveProperty<float> Current { get; set; }
        [field: SerializeField, InlineProperty, ReadOnly]
        public ReactiveProperty<float> Max { get; set; }
        
        public void Reset()
        {
            Current.Value = Starting;
            Max.Value = Starting;
        }
    }
}
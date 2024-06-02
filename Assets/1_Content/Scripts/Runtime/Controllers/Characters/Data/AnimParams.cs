using System;
using UnityEngine;

namespace NJN.Runtime.Controllers.Data
{
    [Serializable]
    public class AnimParams
    {
        [field: SerializeField]
        public string RunBoolName { get; private set; } = "IsRunning";
        [field: SerializeField]
        public string AttackTriggerName { get; private set; } = "Attack";
    }
}
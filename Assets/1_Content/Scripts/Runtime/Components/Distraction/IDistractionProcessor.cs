using System;
using NJN.Runtime.Controllers;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IDistractionProcessor : IComponent
    {
        public float ChanceToResist { get; }
        public float YMaxDistance { get; }
        public float CloseEnough { get; }
        public Vector2 DistractionLocation { get; }

        public void SetLocation(Vector2 location);
    }
}

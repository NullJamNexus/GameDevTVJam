using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IClimbing : IComponent
    {
        public LayerMask LadderLayer{get;}
        public void Climb(Vector2 direction, float speed);
    }
}
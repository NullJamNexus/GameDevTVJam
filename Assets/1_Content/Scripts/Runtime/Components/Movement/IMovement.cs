using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IMovement : IComponent
    {
        public LayerMask LadderLayer { get; }
        public void Move(Vector2 direction, float speed);
    }
}
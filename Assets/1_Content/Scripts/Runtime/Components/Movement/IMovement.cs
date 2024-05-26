using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IMovement : IComponent
    {
        public LayerMask LadderLayer { get; }
        public IClimbable Climbable { get; set; }
        public void Move(Vector2 direction, float speed);
    }
}
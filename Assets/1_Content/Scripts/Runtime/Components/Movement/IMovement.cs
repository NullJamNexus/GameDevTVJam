using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IMovement : IComponent
    {
        public IClimbable Climbable { get; set; }
        public void PhysicsMove(Vector2 direction, bool isSprinting, float? speed = null);
        public void PhysicsHorizontalMove(Vector2 direction, bool isSprinting, float? speed = null);
        public void PhysicsVerticalMove(Vector2 direction, bool isSprinting, float? speed = null);
        public void PhysicsStop();
    }
}
using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IMovement : IComponent
    {
        public void Move(Vector2 direction, float speed);
    }
}
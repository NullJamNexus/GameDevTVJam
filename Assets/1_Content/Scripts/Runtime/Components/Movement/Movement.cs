using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Movement : BaseComponent, IMovement
    {
        public void Move(Vector2 direction, float speed)
        {
            transform.position += (Vector3) direction * speed * Time.deltaTime;
        }
    }
}
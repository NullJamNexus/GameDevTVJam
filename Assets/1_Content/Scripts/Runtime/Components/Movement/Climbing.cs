using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Climbing : BaseComponent, IClimbing
    {
        public LayerMask LadderLayer{get; private set;}
        public void Climb(Vector2 direction, float speed)
        {
            transform.position += new Vector3(0, direction.y * speed * Time.deltaTime, 0);
        }
    }
}

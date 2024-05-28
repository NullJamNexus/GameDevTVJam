using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IPatrolDesignator : IComponent
    {
        public Vector2 GetNextPointDirection(Vector2 position);
        public bool HasArrivedAtPoint(Vector2 position);
    }
}
using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IClimbable
    {
        public Vector2 GetTop();
        public Vector2 GetCenter();
        public Vector2 GetBottom();
    }
}
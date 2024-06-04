using UnityEngine;

namespace NJN.Runtime.Controllers.Player
{
    public struct PlayerTeleportSignal
    {
        public Vector2 NewPosition { get; }
        
        public PlayerTeleportSignal(Vector2 newPosition)
        {
            NewPosition = newPosition;
        }
    }
}
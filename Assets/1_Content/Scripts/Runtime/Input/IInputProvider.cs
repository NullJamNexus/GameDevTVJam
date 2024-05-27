using UnityEngine;

namespace NJN.Runtime.Input
{
    public interface IInputProvider
    {
        // Vector2 Values
        public Vector2 MoveInput { get; }
        public Vector2 MousePosition { get; }
        
        // Buttons
        public InputState InteractInput { get;}
        public InputState SprintInput { get; }

        // Action Maps
        public void EnablePlayerControls();
        public void EnableUIControls();
    }
}
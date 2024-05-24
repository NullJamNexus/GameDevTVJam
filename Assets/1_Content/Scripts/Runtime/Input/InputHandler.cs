using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Input
{
    public class InputHandler : MonoBehaviour, IInputProvider
    {
        private InputSystem _inputActions;

        #region Player Inputs

        // Vector2 Values
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly]
        public Vector2 MoveInput { get; private set; }
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly]
        public Vector2 MousePosition { get; private set; }
        
        // Buttons
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly, InlineProperty]
        public InputState InteractInput { get; private set; }
        
        #endregion

        private void Awake()
        {
            _inputActions = new InputSystem();

            InteractInput = new InputState(_inputActions.Player.Interact);
        }

        private void OnEnable()
        {
            EnableUIControls();
        }

        private void Update()
        {
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            MousePosition = _inputActions.Player.MousePosition.ReadValue<Vector2>();
        }

        private void LateUpdate()
        {
            InteractInput.Reset();
        }
        
        public void EnablePlayerControls()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Enable();
        }
        
        public void EnableUIControls()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
    }
}
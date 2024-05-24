using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Testing
{
    public class VitTest : MonoBehaviour
    {
        [Inject]
        private IPlayerFactory _playerFactory;
        
        [Inject]
        private IInputProvider _inputProvider;
        
        private void Start()
        {
            PlayerController player = _playerFactory.Create();
            player.transform.position = Vector2.zero;
            _inputProvider.EnablePlayerControls();
        }
    }
}
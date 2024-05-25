using System;
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Testing
{
    public class VitTest : MonoBehaviour
    {
        [Inject]
        private ICharacterFactory _characterFactory;
        
        [Inject]
        private IInputProvider _inputProvider;

        private void Start()
        {
            //SpawnPlayer();
        }

        [Button(ButtonSizes.Large)]
        private void SpawnPlayer()
        {
            PlayerController player = _characterFactory.CreatePlayer();
            player.transform.position = Vector2.zero;
            _inputProvider.EnablePlayerControls();
        }
        
        [Button(ButtonSizes.Large)]
        private void SpawnEnemy()
        {
            EnemyController player = _characterFactory.CreateEnemy();
            player.transform.position = Vector2.zero;
        }
    }
}
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using NJN.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private ICharacterFactory _characterFactory;
        private IEnemySpawner _enemySpawner;
        private IInputProvider _inputProvider;
        
        [Inject]
        private void Construct(ICharacterFactory characterFactory, IEnemySpawner enemySpawner, IInputProvider inputProvider)
        {
            _characterFactory = characterFactory;
            _enemySpawner = enemySpawner;
            _inputProvider = inputProvider;
        }
        
        [Button(ButtonSizes.Large)]
        private void SpawnPlayer()
        {
            PlayerController player = _characterFactory.CreatePlayer();
            player.transform.position = Vector2.zero;
            _inputProvider.EnablePlayerControls();
        }
        
        [Button(ButtonSizes.Large)]
        private void StartEnemySpawner()
        {
            _enemySpawner.StartSpawner();
        }
    }
}
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Controllers.Player;
using NJN.Scriptables.Settings;
using Zenject;

namespace NJN.Runtime.Factories
{
    public class CharacterFactory : ICharacterFactory
    {
        private readonly DiContainer _container;
        private readonly LevelSettingsSO _levelSettingsData;
        
        public CharacterFactory(DiContainer container, LevelSettingsSO levelSettingsData)
        {
            _container = container;
            _levelSettingsData = levelSettingsData;
        }
        
        public PlayerController CreatePlayer()
        {
            PlayerController playerInstance = _container.InstantiatePrefabForComponent<PlayerController>(_levelSettingsData.PlayerPrefab);
            return playerInstance;
        }

        public EnemyController CreateEnemy()
        {
            EnemyController enemyInstance = _container.InstantiatePrefabForComponent<EnemyController>(_levelSettingsData.EnemyPrefab);
            return enemyInstance;
        }
    }
}
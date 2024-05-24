using NJN.Runtime.Controllers.Player;
using NJN.Scriptables.Settings;
using Zenject;

namespace NJN.Runtime.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly DiContainer _container;
        private readonly LevelSettingsSO _levelSettingsData;
        
        public PlayerFactory(DiContainer container, LevelSettingsSO levelSettingsData)
        {
            _container = container;
            _levelSettingsData = levelSettingsData;
        }
        
        public PlayerController Create()
        {
            PlayerController playerInstance = _container.InstantiatePrefabForComponent<PlayerController>(_levelSettingsData.PlayerPrefab);
            return playerInstance;
        }
    }
}
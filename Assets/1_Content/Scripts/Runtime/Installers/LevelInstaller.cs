using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Systems;
using NJN.Runtime.Systems.Spawners;
using NJN.Scriptables.Settings;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [BoxGroup("Level Settings"), SerializeField]
        private LevelSettingsSO _levelSettingsData;
        
        public override void InstallBindings()
        {
            // Factories
            Container.Bind<ICharacterFactory>().To<CharacterFactory>().AsSingle().WithArguments(_levelSettingsData).NonLazy();
            Container.Bind<IItemFactory>().To<ItemFactory>().AsSingle().NonLazy();
            
            // Spawners
            Container.BindInterfacesTo<EnemySpawner>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ItemSpawner>().AsSingle().NonLazy();
            
            // Signals
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<ResourceCollectedSignal>();
        }
    }
}
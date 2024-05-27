using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Managers;
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
        
        [BoxGroup("Gloabl UI"), SerializeField]
        private InteractionPrompt _interactionPrompt;
        
        public override void InstallBindings()
        {
            // Factories
            Container.Bind<ICharacterFactory>().To<CharacterFactory>().AsSingle().WithArguments(_levelSettingsData).NonLazy();
            Container.BindInterfacesTo<ItemFactory>().AsSingle().NonLazy();
            
            // Spawners
            Container.BindInterfacesTo<EnemySpawner>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ItemSpawner>().AsSingle().NonLazy();

            //Managers
            Container.BindInterfacesAndSelfTo<LevelManager>().FromComponentsInHierarchy().AsSingle().NonLazy();
            
            //UI
            Container.Bind<InteractionPrompt>().FromComponentInNewPrefab(_interactionPrompt).UnderTransform(transform).AsSingle().NonLazy();
            
            BindLevelSignals();
        }
        
        private void BindLevelSignals()
        {
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<ResourceCollectedSignal>();
            Container.DeclareSignal<CookedFoodSignal>();
            Container.DeclareSignal<DrankWaterSignal>();
            Container.DeclareSignal<ReadNoteSignal>();
        }
    }
}
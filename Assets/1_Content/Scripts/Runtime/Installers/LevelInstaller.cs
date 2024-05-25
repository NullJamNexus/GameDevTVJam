using NJN.Runtime.Factories;
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
            // Player
            Container.Bind<ICharacterFactory>().To<CharacterFactory>().AsSingle().WithArguments(_levelSettingsData);
        }
    }
}
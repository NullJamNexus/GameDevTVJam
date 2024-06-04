using NJN.Runtime.Input;
using NJN.Runtime.Managers;
using NJN.Runtime.Managers.Scenes;
using NJN.Scriptables;
using UnityEngine;
using Vit.Utilities;
using Vit.Utilities.Extensions;
using Zenject;

namespace NJN.Runtime.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Game
            Container.BindInterfacesTo<GameManager>().AsSingle().NonLazy();
            
            // Input
            InputHandler inputHandler = gameObject.GetOrAdd<InputHandler>();
            Container.BindInterfacesTo<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();

            // Audio
            if (Tools.TryLoadResource("Audio/AudioEventBinding", out AudioEventBindingSO audioEventBinding))
                Container.BindInterfacesTo<Managers.AudioManager>()
                    .AsSingle()
                    .WithArguments(audioEventBinding)
                    .NonLazy();
            else
                Debug.LogError("[GameInstaller] Missing or misnamed AudioEventBinding in Resources folder!");
            
            // Scenes
            if (Tools.TryLoadResource("Scenes/SceneSettings", out SceneSettingsSO sceneSettings))
            {
                Container.Bind<SceneLoader>().AsSingle().WithArguments(sceneSettings).NonLazy();
            }
            else
                Debug.LogError("[GameInstaller] Missing or misnamed 'SceneSettings' in Resources folder!");
        }
    }
}
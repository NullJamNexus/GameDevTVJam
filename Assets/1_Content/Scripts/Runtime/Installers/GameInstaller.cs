using NJN.Runtime.Input;
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
            // Input
            InputHandler inputHandler = gameObject.GetOrAdd<InputHandler>();
            Container.BindInterfacesTo<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();

            // Managers
            if (Tools.TryLoadResource("Audio/AudioEventBinding", out AudioEventBindingSO audioEventBinding))
                Container.BindInterfacesTo<Managers.AudioManager>()
                    .AsSingle()
                    .WithArguments(audioEventBinding)
                    .NonLazy();
            else
                Debug.LogError("[GameInstaller] Missing or misnamed AudioEventBinding in Resources folder!");
        }
    }
}
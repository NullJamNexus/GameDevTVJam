using NJN.Runtime.Input;
using Vit.Utilities.Extensions;
using Zenject;

namespace NJN.Runtime.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            // Input
            InputHandler inputHandler = gameObject.GetOrAdd<InputHandler>();
            Container.BindInterfacesTo<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();
        }
    }
}
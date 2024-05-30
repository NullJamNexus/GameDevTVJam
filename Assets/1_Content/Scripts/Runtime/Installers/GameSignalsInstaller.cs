using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Managers;
using NJN.Runtime.UI.Panels;
using Zenject;

namespace NJN.Runtime.Installers
{
    public class GameSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<ResourceCollectedSignal>();
            Container.DeclareSignal<CookedFoodSignal>();
            Container.DeclareSignal<DrankWaterSignal>();
            Container.DeclareSignal<ReadNoteSignal>();
            Container.DeclareSignal<PickDestinationSignal>();
            Container.DeclareSignal<DestinationSelectedSignal>();
            Container.DeclareSignal<FuelDepletedSignal>();
            Container.DeclareSignal<EnteredTruckSignal>();
            Container.DeclareSignal<ExitedTruckSignal>();
            Container.DeclareSignal<PlayerDamageSignal>();
            Container.DeclareSignal<HideSignal>();
            Container.DeclareSignal<EndHideSignal>();
        }
    }
}
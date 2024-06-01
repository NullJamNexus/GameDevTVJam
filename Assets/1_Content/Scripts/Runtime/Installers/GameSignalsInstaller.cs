﻿using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Managers;
using NJN.Runtime.UI.Panels;
using NJN.Runtime.SoundSignal;
using Zenject;
using NJN.Runtime.Controllers;
using NJN.Runtime.Managers.Level.Signals;
using NJN.Runtime.Managers.Scenes.Signals;
using NJN.Runtime.Managers.Signals;

namespace NJN.Runtime.Installers
{
    public class GameSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            // Game Management
            Container.DeclareSignal<BootstrapperInitializedSignal>();
            Container.DeclareSignal<SceneLoadStartedSignal>();
            Container.DeclareSignal<SceneLoadFinishedSignal>();
            Container.DeclareSignal<PlayPressedSignal>();
            Container.DeclareSignal<GameOverSignal>();
            
            // Scenes
            Container.DeclareSignal<LoadingFadeSignal>();
            Container.DeclareSignal<FadeEndSignal>();
            
            // Player
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<PlayerTeleportSignal>();
            Container.DeclareSignal<PlayerDamageSignal>();
            Container.DeclareSignal<PlayerHideSignal>();
            Container.DeclareSignal<PlayerUnhideSignal>();
            Container.DeclareSignal<PlayerClimbSignal>();
            Container.DeclareSignal<PlayerEndClimbSignal>();
            Container.DeclareSignal<PlayerGetDamage>();
            
            // Level
            Container.DeclareSignal<DestinationTransitionStartedSignal>();
            Container.DeclareSignal<DestinationTransitionFinishedSignal>();
            
            // Other
            Container.DeclareSignal<ResourceCollectedSignal>();
            Container.DeclareSignal<CookedFoodSignal>();
            Container.DeclareSignal<DrankWaterSignal>();
            Container.DeclareSignal<ReadNoteSignal>();
            Container.DeclareSignal<PickDestinationSignal>();
            Container.DeclareSignal<DestinationSelectedSignal>();
            Container.DeclareSignal<FuelDepletedSignal>();
            Container.DeclareSignal<EnteredTruckSignal>();
            Container.DeclareSignal<ExitedTruckSignal>();
            Container.DeclareSignal<StopAmbianceSignal>();
            Container.DeclareSignal<MusicSignal>();
            Container.DeclareSignal<EatFoodSignal>();
            Container.DeclareSignal<UseStairsSignal>();
            Container.DeclareSignal<DoorOpenSignal>();
            Container.DeclareSignal<DoorCloseSignal>();
            Container.DeclareSignal<EnemyChaseSignal>();
            Container.DeclareSignal<EndEnemyChaseSignal>();
            Container.DeclareSignal<DrinkCoolerWaterSignal>();
            Container.DeclareSignal<DrinkOutsideWaterSignal>();
        }
    }
}
using FMOD.Studio;
using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Player
{
    public class PlayerAudioEventsHandler 
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public PlayerAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;

            _com.SetInstance(ref _climbInstance, _data.ClimbingLadder);
        }

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<PlayerHideSignal>(OnHide);
            _signalBus.Subscribe<ResourceCollectedSignal>(Collect);
            _signalBus.Subscribe<DrinkOutsideWaterSignal>(Drank);
            _signalBus.Subscribe<ReadNoteSignal>(ReadNote);
            _signalBus.Subscribe<PlayerUnhideSignal>(OffHide);
            _signalBus.Subscribe<CookedFoodSignal>(Cooking);
            _signalBus.Subscribe<PlayerClimbSignal>(Climb);
            _signalBus.Subscribe<PlayerEndClimbSignal>(EndClimb);
            _signalBus.Subscribe<PlayerGetDamage>(GetDamage);
            _signalBus.Subscribe<EatFoodSignal>(EatFood);
            _signalBus.Subscribe<UseStairsSignal>(UseStairs);
        }
        
        public void Dispose()
        {
            ReleaseAllInstances();

            _signalBus.TryUnsubscribe<PlayerHideSignal>(OnHide);
            _signalBus.TryUnsubscribe<ResourceCollectedSignal>(Collect);
            _signalBus.TryUnsubscribe<DrinkOutsideWaterSignal>(Drank);
            _signalBus.TryUnsubscribe<ReadNoteSignal>(ReadNote);
            _signalBus.TryUnsubscribe<PlayerUnhideSignal>(OffHide);
            _signalBus.TryUnsubscribe<CookedFoodSignal>(Cooking);
            _signalBus.TryUnsubscribe<PlayerClimbSignal>(Climb);
            _signalBus.TryUnsubscribe<PlayerEndClimbSignal>(EndClimb);
            _signalBus.TryUnsubscribe<PlayerGetDamage>(GetDamage);
            _signalBus.TryUnsubscribe<EatFoodSignal>(EatFood);
            _signalBus.TryUnsubscribe<UseStairsSignal>(UseStairs);
        }

        #region EventInstances

        private EventInstance _climbInstance;
        #endregion
        private void ReleaseAllInstances()
        {
            _com.RelaeseInstance(ref _climbInstance);
        }

        // Audio events
        private void OnHide(PlayerHideSignal signal)
        {
            _com.PlayOneShot(_data.Hide);
        }
        private void Collect(ResourceCollectedSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.GatherResource);
        }
        private void Drank()
        {
            RuntimeManager.PlayOneShot(_data.DrinkingWater);
        }
        private void ReadNote(ReadNoteSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.PickUpANote);
        }
        private void OffHide(PlayerUnhideSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.EndHide);
        }
        private void Cooking(CookedFoodSignal signal)
        {
           RuntimeManager.PlayOneShot(_data.CookingOnStove);
        }

        private void Climb()
        {
            _com.ContinueInstance(ref _climbInstance);
        }

        private void EndClimb()
        {
            _com.StopInstance(ref _climbInstance);
        }

        private void GetDamage()
        {
            _com.PlayOneShot(_data.TakeDamage);
        }

        private void EatFood()
        {
            _com.PlayOneShot(_data.EatingFood);
        }

        private void UseStairs()
        {
            _com.PlayOneShot(_data.UsingStairs);
        }
    }
}
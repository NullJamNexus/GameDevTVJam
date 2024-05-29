using FMODUnity;
using NJN.Runtime.Components;
using NJN.Scriptables.Audio.Player;
using UnityEngine;
using Zenject;

namespace AudioManager.Player
{
    public class PlayerAudioManager 
    {
        private SignalBus _signalBus;
        private PlayerAudioEvents _data;
        public PlayerAudioManager(SignalBus signalBus, PlayerAudioEvents playerAudioEvents)
        {
            _signalBus = signalBus;
            _data = playerAudioEvents;
            SubscribeToSignals();
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<HideSignal>(OnHide);
        }

        private void OnHide(HideSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.Hide);
        }
        public void Destroy()
        {

        }
    }
}
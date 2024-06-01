using FMOD.Studio;
using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Fmod;
using NJN.Runtime.SoundSignal;
using NJN.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AudioManager.Ambiance
{
    enum EAmbiance { stop, truck, outside, building }
    public class AmbianceAudioEventsHandler
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public AmbianceAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;
            _currentAmbiance = EAmbiance.stop;
        }

        private EAmbiance _currentAmbiance;
        private EventInstance _truckInstance;
        private EventInstance _outsideInstance;
        private EventInstance _buildingInstance;

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<EnteredTruckSignal>(InTruck);
            _signalBus.Subscribe<ExitedTruckSignal>(Outside);
            _signalBus.Subscribe<StopAmbianceSignal>(StopAmbiance);

            // connect enter exit building

        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<EnteredTruckSignal>(InTruck);
            _signalBus.TryUnsubscribe<ExitedTruckSignal>(Outside);
            _signalBus.TryUnsubscribe<StopAmbianceSignal>(StopAmbiance);

            ReleaseAllInstances();
        }

        private void ReleaseAllInstances()
        {
            _com.RelaeseInstance(ref _truckInstance);
            _com.RelaeseInstance(ref _outsideInstance);
            _com.RelaeseInstance(ref _buildingInstance);
        }
        
        private void InTruck()
        {
            ChangeAmbiance(EAmbiance.truck);
        }
        private void InBuilding()
        {
            ChangeAmbiance(EAmbiance.building);
        }

        private void Outside()
        {
            ChangeAmbiance(EAmbiance.outside);
        }

        private void StopAmbiance()
        {
            ChangeAmbiance(EAmbiance.stop);
        }
        private void ChangeAmbiance(EAmbiance ambiance)
        {
            if (_currentAmbiance == ambiance)
                return;
        
            if(_currentAmbiance != EAmbiance.stop)
            {
                _com.RelaeseInstance(ref GetCurrentInstance());
            }
           
            _currentAmbiance = ambiance;
            if(_currentAmbiance != EAmbiance.stop)
            {
                _com.SetInstanceAndPlay(ref GetCurrentInstance(), GetReferance());
            }
        }

        private ref EventInstance GetCurrentInstance()
        {
            switch(_currentAmbiance)
            {
                case EAmbiance.truck:
                    return ref _truckInstance;
                case EAmbiance.outside:
                    return ref _outsideInstance;
                case EAmbiance.building:
                    return ref _buildingInstance;
                default:
                    return ref _truckInstance;
            }
        }

        private EventReference GetReferance()
        {
            switch (_currentAmbiance)
            {
                case EAmbiance.truck:
                    return _data.Amb_inTruck;
                case EAmbiance.outside:
                    return _data.Amb_outside;
                case EAmbiance.building:
                    return _data.Amb_inBuilding;
                default:
                    return _data.Amb_inBuilding;
            }
        }

        // Audio events

    }
}
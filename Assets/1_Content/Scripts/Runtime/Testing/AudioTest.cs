using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers;
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.SoundSignal;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.Testing
{
    public class AudioTest : MonoBehaviour
    {
        [SerializeField]
        private EMusic _music;


        private SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        [Button(ButtonSizes.Large)]
        private void PlaySelectedMusic()
        {
            _signalBus.Fire<MusicSignal>(new MusicSignal(_music));
        }

        [Button(ButtonSizes.Large)]
        private void EntertruckAmbiance()
        {
            _signalBus.Fire(new EnteredTruckSignal());
        }
        [Button(ButtonSizes.Large)]
        private void ExittruckAmbiance()
        {
            _signalBus.Fire(new ExitedTruckSignal());
        }
        [Button(ButtonSizes.Large)]
        private void StopAmbiance()
        {
            _signalBus.Fire(new StopAmbianceSignal());
        }

        [Button(ButtonSizes.Large)]
        private void FireDesiredSignal()
        {
            _signalBus.Fire(new EatFoodSignal());
        }

    }
}
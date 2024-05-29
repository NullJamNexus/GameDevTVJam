using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.Testing
{
    public class YaygunTest : MonoBehaviour
    {
        [SerializeField, ValueDropdown("GetLayerNames")]
        private string _selectedLayer;

        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }

        private SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        [Button(ButtonSizes.Large)]
        private void TeleportPlayer()
        {
            EnemyController enemy = GameObject.FindAnyObjectByType<EnemyController>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = Vector3.zero;
        }

        [Button(ButtonSizes.Large)]
        private void Entertruck()
        {
            _signalBus.Fire(new EnterTruckSignal());
        }
        [Button(ButtonSizes.Large)]
        private void Exittruck()
        {
            _signalBus.Fire(new ExitTruckSignal());
        }

    }
}
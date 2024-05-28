using System;
using System.Collections;
using NJN.Runtime.Components;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
namespace NJN.Runtime.Managers
{
    public class OxygenManager : MonoBehaviour
    {
        [field: FoldoutGroup("TimeValues"), SerializeField]
        private float _oxygenFullTime;
        [field: FoldoutGroup("TimeValues"), SerializeField]
        private float _oxygenRegainFrequence;
        [field: FoldoutGroup("DamageValues"), SerializeField]
        private float _delayBetweenDamage;
        [field: FoldoutGroup("DamageValues"), SerializeField]
        private float _damage;

        [field: FoldoutGroup("Text"), SerializeField]
        private TextMeshProUGUI _textTimer;
        [field: FoldoutGroup("Text"), SerializeField]
        private string _RemainingTimeText = "Remaining time : ";

        private float _remainingOxygenTime;

        private SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void OnEnable()
        {
            _signalBus.Subscribe<EnterTruckSignal>(OnEnterTruck);
            _signalBus.Subscribe<ExitTruckSignal>(OnExitTruck);
            _signalBus.Subscribe<PickDestinationSignal>(OnReachDestination);
        }

        private void Start()
        {
            _remainingOxygenTime = _oxygenFullTime;
        }
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<EnterTruckSignal>(OnEnterTruck);
            _signalBus.TryUnsubscribe<ExitTruckSignal>(OnExitTruck);
            _signalBus.TryUnsubscribe<PickDestinationSignal>(OnReachDestination);
        }
        [Button(ButtonSizes.Large)]
        private void OnExitTruck()
        {
            StopAllCoroutines();
            StartCoroutine(LoseOxygen());
        }
        [Button(ButtonSizes.Large)]
        private void OnEnterTruck()
        {
            StopAllCoroutines();
            StartCoroutine(RegainOxygen());
        }
        [Button(ButtonSizes.Large)]
        private void OnReachDestination()
        {
            StopAllCoroutines();
            _remainingOxygenTime = _oxygenFullTime;
            PrintTime();
        }

        private void StartDamage()
        {
            StopAllCoroutines();
            StartCoroutine(GetDamage());
        }
        private IEnumerator LoseOxygen()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                _remainingOxygenTime--;
                PrintTime();

                if (_remainingOxygenTime < 0 )
                {
                    _remainingOxygenTime = 0;
                    StartDamage();
                }
            }
        }

        private IEnumerator RegainOxygen()
        {
            while (true)
            {
                yield return new WaitForSeconds(_oxygenRegainFrequence);
                _remainingOxygenTime = math.min(++_remainingOxygenTime, _oxygenFullTime);
                PrintTime();
            }
        }

        private IEnumerator GetDamage()
        {
            while (true)
            {
                // give damage here
                print("give damage " + _damage);
                yield return new WaitForSeconds(_delayBetweenDamage);
            }
        }

        private void PrintTime()
        {
            _textTimer.text = _RemainingTimeText + GetTimeAsString();
        }

        private string GetTimeAsString()
        {
            int time = (int)_remainingOxygenTime;
            string text = $"{time / 60} : {time % 60}";
            if (0 == time % 60)
                text += "0";
            return text;
        }

    }
}

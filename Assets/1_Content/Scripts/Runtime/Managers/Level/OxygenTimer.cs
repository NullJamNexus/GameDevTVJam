using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using MEC;

namespace NJN.Runtime.Managers
{
    public class OxygenTimer : MonoBehaviour, IOxygenTimer
    {
        [field: FoldoutGroup("TimeValues"), SerializeField]
        private float _oxygenFullTime = 300f;
        [field: FoldoutGroup("TimeValues"), SerializeField]
        private float _oxygenRegainFrequency = 1f;
        [field: FoldoutGroup("DamageValues"), SerializeField]
        private float _delayBetweenDamage = 1f;
        [field: FoldoutGroup("DamageValues"), SerializeField]
        private float _damage = 5f;

        // TODO: Move this to the appropriate UI class...
        [field: FoldoutGroup("Text"), SerializeField]
        private TextMeshProUGUI _textTimer;
        [field: FoldoutGroup("Text"), SerializeField]
        private string _remainingTimeText = "Remaining time : ";

        private float _remainingOxygenTime;
        private SignalBus _signalBus;
        private CoroutineHandle _oxygenCoroutineHandle;
        private CoroutineHandle _damageCoroutineHandle;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            // TODO: This is temporary unity dependency resolution while UI is in here...
            if (_textTimer == null)
                _textTimer = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnteredTruckSignal>(OnEnterTruck);
            _signalBus.Subscribe<ExitedTruckSignal>(OnExitTruck);
            _signalBus.Subscribe<PickDestinationSignal>(OnReachDestination);
        }

        private void Start()
        {
            _remainingOxygenTime = _oxygenFullTime;
            UpdateTimerText();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<EnteredTruckSignal>(OnEnterTruck);
            _signalBus.TryUnsubscribe<ExitedTruckSignal>(OnExitTruck);
            _signalBus.TryUnsubscribe<PickDestinationSignal>(OnReachDestination);
        }

        private void OnExitTruck()
        {
            Timing.KillCoroutines(_oxygenCoroutineHandle);
            _oxygenCoroutineHandle = Timing.RunCoroutine(LoseOxygen());
        }

        private void OnEnterTruck()
        {
            Timing.KillCoroutines(_oxygenCoroutineHandle);
            _oxygenCoroutineHandle = Timing.RunCoroutine(RegainOxygen());
        }

        private void OnReachDestination()
        {
            Timing.KillCoroutines(_oxygenCoroutineHandle);
            Timing.KillCoroutines(_damageCoroutineHandle);
            _remainingOxygenTime = _oxygenFullTime;
            UpdateTimerText();
        }

        private void StartDamage()
        {
            Timing.KillCoroutines(_damageCoroutineHandle);
            _damageCoroutineHandle = Timing.RunCoroutine(ApplyDamage());
        }

        private IEnumerator<float> LoseOxygen()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                _remainingOxygenTime--;
                UpdateTimerText();

                if (_remainingOxygenTime <= 0)
                {
                    _remainingOxygenTime = 0;
                    StartDamage();
                    yield break;
                }
            }
        }

        private IEnumerator<float> RegainOxygen()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(_oxygenRegainFrequency);
                _remainingOxygenTime = math.min(_remainingOxygenTime + 1, _oxygenFullTime);
                UpdateTimerText();
            }
        }

        private IEnumerator<float> ApplyDamage()
        {
            while (true)
            {
                _signalBus.Fire(new PlayerDamageSignal(_damage));
                yield return Timing.WaitForSeconds(_delayBetweenDamage);
            }
        }

        private void UpdateTimerText()
        {
            _textTimer.text = $"{_remainingTimeText}{GetTimeAsString()}";
        }

        private string GetTimeAsString()
        {
            int time = Mathf.CeilToInt(_remainingOxygenTime);
            return $"{time / 60:D2}:{time % 60:D2}";
        }
    }
}

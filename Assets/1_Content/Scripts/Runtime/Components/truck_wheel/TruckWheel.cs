using NJN.Runtime.Managers.Level.Signals;
using System.Collections;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class TruckWheel : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private float _speed;
        readonly private float _speedMultiplyConstant = 50;
        readonly private float _accelerationMultiplyConstant = 30;
        readonly private float _deaccelerationMultiplyConstant = 3.07f;
  
        private void Start()
        {
            _signalBus.Subscribe<DestinationTransitionStartedSignal>(TransitionStarted);
            _signalBus.Subscribe<DestinationTransitionFinishedSignal>(TransitionEnded);
        }
        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<DestinationTransitionStartedSignal>(TransitionStarted);
            _signalBus.TryUnsubscribe<DestinationTransitionFinishedSignal>(TransitionEnded);
        }

        private void TransitionStarted(DestinationTransitionStartedSignal signal)
        {
            StartCoroutine(Acceleration(signal.AccelerationRate, signal.MaxSpeed));
        }

        private void TransitionEnded(DestinationTransitionFinishedSignal signal)
        {
            StopAllCoroutines();
            StartCoroutine(Deceleration(signal.DecelerationRate));
        }

        private IEnumerator Acceleration(float accelerate, float maxSpeed)
        {
            while (true)
            {
                _speed += accelerate * _accelerationMultiplyConstant * Time.deltaTime;
                _speed = Mathf.Min(maxSpeed, _speed);
                transform.Rotate(0, 0, _speed * _speedMultiplyConstant * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator Deceleration(float DecelerationRate)
        {
            while (_speed > 0)
            {
                transform.Rotate(0, 0, _speed * _speedMultiplyConstant * Time.deltaTime);
                _speed -= DecelerationRate * _deaccelerationMultiplyConstant * Time.deltaTime;
                yield return null;
            }
            _speed = 0;
        }
    }
}
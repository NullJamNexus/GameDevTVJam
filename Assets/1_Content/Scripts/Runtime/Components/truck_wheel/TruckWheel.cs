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

        private float _rotateSpeed=360;

        private void Start()
        {
            
        }
        private void Update()
        {
            transform.Rotate(0,0,_rotateSpeed * Time.deltaTime);
        }
        private void OnDestroy()
        {
            
        }

        private void SetSpeed()
        {

        }
    }
}
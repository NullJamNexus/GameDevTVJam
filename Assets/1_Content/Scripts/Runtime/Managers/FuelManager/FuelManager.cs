using NJN.Runtime.Components;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
namespace NJN.Runtime.Managers
{
    public class FuelManager : MonoBehaviour
    {
        [field: FoldoutGroup("Values"), SerializeField]
        public float MaxFuel { get; private set; }

        [field: FoldoutGroup("Values"), SerializeField]
        private float _depletionAmount { get; set; }

        public float CurrentFuel { get; private set; }

        private SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<AddFuelSignal>(OnAddFuel);
        }

        private void Start()
        {
            CurrentFuel = MaxFuel;
        }
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<AddFuelSignal>(OnAddFuel);
        }

        private void Update()
        {
            if(CurrentFuel <= 0)
            {
                CurrentFuel = 0;
                FuelOver();
            }
            else
            {
                CurrentFuel -= _depletionAmount * Time.deltaTime;
            }
        }

        private void OnAddFuel(AddFuelSignal signal)
        {
            CurrentFuel = math.min(CurrentFuel + signal.Amount,MaxFuel);
        }
        private void FuelOver()
        {
            print("Fuel Over Game over");
            _signalBus.Fire<FuelOverSignal>();
        }

    }
}

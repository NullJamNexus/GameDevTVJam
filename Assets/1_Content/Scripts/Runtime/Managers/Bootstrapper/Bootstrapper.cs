using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using MEC;
using NJN.Runtime.Managers.Signals;
using Zenject;

namespace NJN.Runtime.Managers.Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [BankRef]
        public List<string> Banks = new ();
        private bool _banksLoaded;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            Timing.RunCoroutine(InitializeGame().CancelWith(this));
        }
        
        private IEnumerator<float> InitializeGame()
        {
            FmodLoader fmodLoader = GetComponent<FmodLoader>();
            if (fmodLoader != null)
            {
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(fmodLoader.LoadBanks().CancelWith(fmodLoader)));
            }
            else
            {
                Debug.LogWarning("FmodLoader not found.");
            }

            _signalBus.Fire(new BootstrapperInitializedSignal());
        }
    }
}
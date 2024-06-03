using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using MEC;
using NJN.Runtime.Managers.Signals;
using Zenject;
using System.Collections;
using UnityEngine.SceneManagement;
using NJN.Runtime.SoundSignal;
using NJN.Runtime.Components;


namespace NJN.Runtime.Managers.Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private float _fmodInitializationDelay = 1f;

        private SignalBus _signalBus;
        private CoroutineHandle _initializeGameHandle;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            //_initializeGameHandle = Timing.RunCoroutine(InitializeGame().CancelWith(this));
            //SceneManager.LoadScene("2_MainMenu", LoadSceneMode.Single);
            StartCoroutine(InitializeGameUnity());
        }

        private IEnumerator<float> InitializeGame()
        {
            yield return Timing.WaitForSeconds(_fmodInitializationDelay);

            FmodLoader fmodLoader = GetComponent<FmodLoader>();
            if (fmodLoader != null)
            {
                //yield return Timing.WaitUntilDone(Timing.RunCoroutine(fmodLoader.LoadBanks().CancelWith(fmodLoader)));
                yield return Timing.WaitForOneFrame;
            }
            else
            {
                Debug.LogWarning("FmodLoader not found.");
            }

            Timing.KillCoroutines(_initializeGameHandle);
            _signalBus.Fire(new BootstrapperInitializedSignal());
        }

        private IEnumerator InitializeGameUnity()
        {
            yield return new WaitForSeconds(_fmodInitializationDelay);

            FmodLoader fmodLoader = GetComponent<FmodLoader>();
            if (fmodLoader != null)
            {
                //yield return Timing.WaitUntilDone(Timing.RunCoroutine(fmodLoader.LoadBanks().CancelWith(fmodLoader)));
                yield return null;
            }
            else
            {
                Debug.LogWarning("FmodLoader not found.");
            }

            KillIt();
        }

        private void KillIt()
        {
            StopAllCoroutines();
            SceneManager.LoadScene("2_MainMenu", LoadSceneMode.Single);
            _signalBus?.Fire(new MusicSignal(EMusic.truck));
            _signalBus?.Fire(new ExitBuildingSignal());
            //_signalBus.Fire(new BootstrapperInitializedSignal());
        }
    }
}
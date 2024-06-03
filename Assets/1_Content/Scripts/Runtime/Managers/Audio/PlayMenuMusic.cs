using NJN.Runtime.Components;
using NJN.Runtime.SoundSignal;
using UnityEngine;
using Zenject;

public class PlayMenuMusic : MonoBehaviour
{
    public SignalBus _signalBus { get; private set; }
    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    void Start()
    {
        _signalBus.Fire(new MusicSignal(EMusic.truck));
        _signalBus.Fire(new ExitBuildingSignal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

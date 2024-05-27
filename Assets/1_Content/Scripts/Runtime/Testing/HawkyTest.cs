
using NJN.Runtime.Managers;
using UnityEngine;
using Zenject;


public class HawkyTest : MonoBehaviour
    {

    private LevelManager _levelManager;

    [Inject]
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

        
    }


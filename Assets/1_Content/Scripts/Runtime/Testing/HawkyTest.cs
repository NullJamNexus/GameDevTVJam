using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;
using ModestTree;
using UnityEditorInternal;
using Mono.CSharp;
using Unity.VisualScripting;
using NJN.Runtime.Components;
using System.Runtime.CompilerServices;

namespace NJN.Runtime.Testing{


public class HawkyTest : MonoBehaviour
{
        [Inject]
        private IPlayerFactory _playerFactory;
        
        [Inject]
        private IInputProvider _inputProvider;

        PlayerController player;

        [SerializeField] bool playerIsClimbing;
    


    [Button("Spawn Player")]
    private void SpawnPlayer(){
        player = _playerFactory.Create();
        player.transform.position = Vector2.zero;
         _inputProvider.EnablePlayerControls();

    }

    private void Update(){
        
       
    }

    
}
}

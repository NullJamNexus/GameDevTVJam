
using NJN.Runtime.Controllers.Player;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;
using System.Runtime.CompilerServices;

public class ParallaxBG : MonoBehaviour
{
    //TODO:
    //Add serialized sprites so we can plug background art in later

    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;
    private LevelManager _levelManager;
    private Transform _player;

    [SerializeField] private float speedOffset;
    

    [Inject]
    //TODO:
    //Change level manager to interface
    private void Construct(LevelManager levelManager){
        _levelManager = levelManager;
        //Debug.Log(_player);
    }

    private void Awake(){
        _layerBack = gameObject.transform.GetChild(0).gameObject;
        _layerMiddle = gameObject.transform.GetChild(1).gameObject;
        _layerFront = gameObject.transform.GetChild(2).gameObject;
    }

  

    private void Update(){
       _layerBack.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / speedOffset);
       _layerMiddle.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/2));
       _layerFront.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/4));
    }

    
}

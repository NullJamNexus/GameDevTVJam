
using NJN.Runtime.Controllers.Player;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using System.Collections.Generic;
using MEC;
using System.Collections;

public class ParallaxBG : MonoBehaviour
{
    //TODO:
    //Add serialized sprites so we can plug background art in later

  
    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;
    private LevelManager _levelManager;

    private bool _parallaxActive;


    List<GameObject> _layerBackChildren = new List<GameObject>();

    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("The distance difference between player and sprite before it is teleported")] private int _distanceDifferential;
    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("How far the sprite needs to be teleported")] private int _distanceToMoveSprite;
    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("Higher number = slower")] private float speedOffset;
    

    [Inject]
    //TODO:
    //Change level manager to interface
    private void Construct(LevelManager levelManager){
        _levelManager = levelManager;
        //Debug.Log(_player);
    }

    private void Awake()
    {
        _layerBack = gameObject.transform.GetChild(0).gameObject;
        if(!(_layerBack.transform.childCount<3)){

            _layerBackChildren.Add(_layerBack.transform.GetChild(0).gameObject);
            _layerBackChildren.Add(_layerBack.transform.GetChild(1).gameObject);
            _layerBackChildren.Add(_layerBack.transform.GetChild(2).gameObject);

        }

        else Debug.Log("PARALLAXBG: Layer has less than three children!");
        
        
        _layerMiddle = gameObject.transform.GetChild(1).gameObject;
        _layerFront = gameObject.transform.GetChild(2).gameObject;

        StartParallax();
    }

    private void Start()
    {
        Timing.RunCoroutine(CheckIfSpriteNeedsMovingCoroutine().CancelWith(gameObject));
    }

        
    private void Update()
    {
        if(_parallaxActive) ScrollLayers();
        
    }

    private void ScrollLayers()
    {
        //TODO:
        //does this need delta time?
        _layerBack.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / speedOffset);
        _layerMiddle.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/2));
        _layerFront.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/4));
    }

    private IEnumerator<float> CheckIfSpriteNeedsMovingCoroutine()
    {
        //wait for one frame to ensure player is instantiated
        yield return Timing.WaitForOneFrame;

        while (_parallaxActive)
        {
            foreach (GameObject child in _layerBackChildren)
            {
                if(child.transform.position.x - _levelManager.Player.transform.position.x > _distanceDifferential)
                {
                    Debug.Log("PARALLAXBG: Sprite moved too far!");
                    child.transform.position = new Vector2(child.transform.position.x - _distanceToMoveSprite , child.transform.position.y);
                }

                else if (child.transform.position.x - _levelManager.Player.transform.position.x < -_distanceDifferential)
                {
                    Debug.Log("PARALLAXBG: Sprite moved too far!");
                    child.transform.position = new Vector2(child.transform.position.x + _distanceToMoveSprite , child.transform.position.y);
                }

                //Debug.Log(child.transform.position.x - _levelManager.Player.transform.position.x);

                //slight delay between checking children to prevent stuttering
                yield return Timing.WaitForSeconds(0.3f);
            }
        } 
        
    }
[Button(ButtonSizes.Large)]
    public void StartParallax()
    {
        if(!_parallaxActive) _parallaxActive=true;
        Timing.RunCoroutine(CheckIfSpriteNeedsMovingCoroutine().CancelWith(gameObject));
        
    }
[Button(ButtonSizes.Large)]
    public void StopParallax()
    {
        if(_parallaxActive) _parallaxActive=false;
    }
}


    



    


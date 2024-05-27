

using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;

using System.Collections.Generic;
using MEC;


public class ParallaxBG : MonoBehaviour
{
    //TODO:
    //Add serialized sprites so we can plug background art in later

    private enum SideOfSprite{
        Left,
        Right
    }

    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;
    private LevelManager _levelManager;

    private bool _parallaxActive;
    private CoroutineHandle _coroutineHandler;

    List<GameObject> _layerBackChildrenList;
    List<GameObject> _layerMiddleChildrenList;
    List<GameObject> _layerFrontChildrenList;

    [InfoBox("ALL SPRITES MUST BE SAME SIZE")]

    //TODO:
    //these can be made relative to sprite width, not sure how to do that at the moment
    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("The distance difference between player and sprite before it is teleported, usually works well with (spritewidth + 10)")] private int _distanceDifferential;
    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("How far the sprite needs to be teleported, usually works well with ( (spritewidth +10) * 2)")] private int _distanceToMoveSprite;
    [SerializeField,BoxGroup("Tweak Values (Read Tooltips)"),Tooltip("Lower number makes effect stronger and faster, recommended to keep around 10")] private float speedOffset;
    

    [Inject]
    //TODO:
    //Change level manager to interface
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
        //Debug.Log(_player);
    }


    private void Start()
    {
        _layerBack = gameObject.transform.GetChild(0).gameObject;
        _layerMiddle = gameObject.transform.GetChild(1).gameObject;
        _layerFront = gameObject.transform.GetChild(2).gameObject;
        
        _layerBackChildrenList = CreateChildrenList(_layerBack);
        _layerMiddleChildrenList = CreateChildrenList(_layerMiddle);
        _layerFrontChildrenList = CreateChildrenList(_layerFront);
        
        ToggleParallax();
    }

    private List<GameObject> CreateChildrenList(GameObject layer){
        
        List<GameObject> newList = new List<GameObject>();
        //check that there is exactly 3 children
        if(layer.transform.childCount==3){
            //add children to list
            newList.Add(layer.transform.GetChild(0).gameObject);
            newList.Add(layer.transform.GetChild(1).gameObject);
            newList.Add(layer.transform.GetChild(2).gameObject);
            return newList;

        }
        else
        {
            Debug.Log($"PARALLAXBG: Layer {layer} has wrong amount of children!");
            return null;
        } 
        
    }

        
    private void Update()
    {
        if(_parallaxActive) ScrollLayers();
        
    }

    private void ScrollLayers()
    {
        //TODO:
        //does this need delta time because its being called in update?
        //needs to take camera position instead of player position since camera is not always fixed on the player
        _layerBack.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / speedOffset);
        _layerMiddle.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/2) );
        _layerFront.transform.position = -(new Vector2(_levelManager.Player.transform.position.x,0) / (speedOffset/4) );
    }

    private IEnumerator<float> MovingSpritesCoroutine()
    {
        //wait for one frame to ensure player is instantiated
        yield return Timing.WaitForOneFrame;

        while (_parallaxActive)
        {
            AdjustSpritePositions(_layerBackChildrenList);
            
            AdjustSpritePositions(_layerMiddleChildrenList);
            
            AdjustSpritePositions(_layerFrontChildrenList);

            yield return Timing.WaitForSeconds(1);
        } 
        
    }

    private void AdjustSpritePositions(List<GameObject> listOfSprites){
        //Debug.Log($"PARALLAXBG: Called adjust sprite position method with this list: {listOfSprites[0]}, {listOfSprites[1]}, {listOfSprites[2]},");
            foreach (GameObject spriteObj in listOfSprites)
            {
                if(spriteObj.transform.position.x - _levelManager.Player.transform.position.x > _distanceDifferential)
                {
                    MoveSprite(SideOfSprite.Left,spriteObj);
                }

                else if (spriteObj.transform.position.x - _levelManager.Player.transform.position.x < -_distanceDifferential)
                {
                    MoveSprite(SideOfSprite.Right,spriteObj);
                }
                
            }
    }

    private void MoveSprite(SideOfSprite side, GameObject spriteObj){

        switch(side)
        {

            case SideOfSprite.Left:
                spriteObj.transform.position = new Vector2(spriteObj.transform.position.x - _distanceToMoveSprite , spriteObj.transform.position.y);
            break;

            case SideOfSprite.Right:
                spriteObj.transform.position = new Vector2(spriteObj.transform.position.x + _distanceToMoveSprite , spriteObj.transform.position.y);
            break;

        }
        
    }


[Button(ButtonSizes.Large)]
    public void ToggleParallax()
    {

        if(Timing.IsRunning(_coroutineHandler))
        {
            Timing.KillCoroutines(_coroutineHandler);
            _parallaxActive=false;
            Debug.Log("PARALLAXBG: Stopped Parallax");
        } 
        else
        {
            _coroutineHandler = Timing.RunCoroutine(MovingSpritesCoroutine().CancelWith(gameObject));
            _parallaxActive=true;
            Debug.Log("PARALLAXBG: Started Parallax");
        } 
        
            
        
        
    }

}


    



    


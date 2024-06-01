using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;
using System.Collections.Generic;
using MEC;
using Unity.Cinemachine;


public class ParallaxBG : MonoBehaviour
{
    private enum SideOfSprite
    {
        Left,
        Right
    }
    
    //TODO: Add serialized sprites so we can plug background art in later

    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;
    
    //TODO: These can be made relative to sprite width, not sure how to do that at the moment
    [InfoBox("ALL SPRITES MUST BE SAME SIZE")]
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("The distance difference between player and sprite before it is teleported, usually works well with (spritewidth + 10)")]
    private int _distanceDifferential;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("How far the sprite needs to be teleported, usually works well with ( (spritewidth +10) * 2)")]
    private int _distanceToMoveSprite;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("Lower number makes effect stronger and faster, recommended to keep around 10")]
    public float _speedOffset = 10f;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("How frequently sprites are translated when player reaches outer bounds (in seconds)")]
    private float _updateFrequency = 0.7f;

    private LevelManager _levelManager;
    private bool _parallaxActive;
    private CoroutineHandle _coroutineHandler;
    private List<GameObject> _layerBackChildren;
    private List<GameObject> _layerMiddleChildren;
    private List<GameObject> _layerFrontChildren;
    private float _startY;
    private bool _autoScroll = true;
    [SerializeField, ReadOnly] public float _autoScrollIncrement;
    private float _currentScrollPos;
    [SerializeField, ReadOnly] private GameObject _centreObj;
    private float _rampUpDown;

    private CinemachineCamera _followCam;

    // TODO: Change LevelManager to its interface
    [Inject]
    private void Construct(LevelManager levelManager, [Inject(Id = "FollowCamera")] CinemachineCamera followCam)
    {
        _levelManager = levelManager;
        _followCam = followCam;
    }

    private void Start()
    {
        InitializeLayers();
        Timing.RunCoroutine(InitializeCentreObject());
        
    }

    private IEnumerator<float> InitializeCentreObject()
    {
        while(_centreObj == null)
        {
            try
            {
                _centreObj = _followCam.gameObject;
            }
            catch
            {
                Debug.Log("Parallax centre object not found, this is most likely due to the object not yet being instantiated.");
            }
            
            yield return Timing.WaitForSeconds(0.1f);
        }
        
    }

    private void OnEnable()
    {
        ForceParallaxOn();

/*         if(_autoScroll)
        {
            SetUpAutoScroll();
        } */
        //Debug.Log(_parallaxActive);
        
    }

/*     private void SetUpAutoScroll()
    {
            _startY = gameObject.transform.position.y;
    } */

    private void InitializeLayers()
    {
        _layerBack = transform.GetChild(0).gameObject;
        _layerMiddle = transform.GetChild(1).gameObject;
        _layerFront = transform.GetChild(2).gameObject;

        _layerBackChildren = GetLayerChildren(_layerBack);
        _layerMiddleChildren = GetLayerChildren(_layerMiddle);
        _layerFrontChildren = GetLayerChildren(_layerFront);
        
    }

    private List<GameObject> GetLayerChildren(GameObject layer)
    {
        List<GameObject> children = new List<GameObject>();
        if (layer.transform.childCount == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                children.Add(layer.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"PARALLAXBG: Layer {layer.name} has the wrong number of children!");
        }
        return children;
    }

    private void Update()
    {
        if (_parallaxActive && !_autoScroll)
        {
            ScrollLayers();
        }
        else if (_parallaxActive && _autoScroll)
        {
            if(_centreObj != null){
                /* _centreObj.transform.position = new Vector2(0,_startY); */
                ScrollLayersTransition(_currentScrollPos);
                _currentScrollPos+=_autoScrollIncrement;
            }
        }
    }

    private void ScrollLayers()
    {
        //TODO: Does this need delta time because its being called in update?
        //THIS IS NOW OBSOLETE KEEPING FOR TROUBLESHOOTING

        float centreObjPosX = _followCam.transform.position.x;
        _layerBack.transform.position = -(new Vector2(centreObjPosX, 0) / _speedOffset);
        _layerMiddle.transform.position = -(new Vector2(centreObjPosX, 0) / (_speedOffset / 3));
        _layerFront.transform.position = -(new Vector2(centreObjPosX, 0) / (_speedOffset / 6));


    }

    private void ScrollLayersTransition(float increment)
    {
        float centreObjPosX = _followCam.transform.position.x;
        _layerBack.transform.position = new Vector2((-centreObjPosX + increment) / _speedOffset, _layerBack.transform.position.y) ;
        _layerMiddle.transform.position = new Vector2((-centreObjPosX + increment) / (_speedOffset / 2) , _layerMiddle.transform.position.y) ;
        _layerFront.transform.position = new Vector2((-centreObjPosX + increment) / (_speedOffset / 4), _layerFront.transform.position.y) ;
    }

    private IEnumerator<float> MovingSpritesCoroutine()
    {
        yield return Timing.WaitForOneFrame;

        while (_parallaxActive)
        {
            AdjustSpritePositions(_layerBackChildren);
            AdjustSpritePositions(_layerMiddleChildren);
            AdjustSpritePositions(_layerFrontChildren);

            yield return Timing.WaitForSeconds(_updateFrequency);
        }
    }

    private void AdjustSpritePositions(List<GameObject> layerChildren)
    {
        foreach (GameObject child in layerChildren)
        {
            float distanceToCentreObj;
            if(!_autoScroll) distanceToCentreObj = child.transform.position.x - _levelManager.Player.transform.position.x;

            else
            {
                if(_centreObj!=null)
                {
                    distanceToCentreObj = child.transform.position.x - _centreObj.transform.position.x;
                }
                else
                {
                    distanceToCentreObj = 0;
                    Debug.Log($"{gameObject} is trying to access a centre object which is null! If the player is not yet instantiated, this is normal");
                }
                
            } 

            if (distanceToCentreObj > _distanceDifferential)
            {
                MoveSprite(SideOfSprite.Left, child);
            }
            else if (distanceToCentreObj < -_distanceDifferential)
            {
                MoveSprite(SideOfSprite.Right, child);
            }
        }
    }

    private void MoveSprite(SideOfSprite side, GameObject spriteObj)
    {
        float moveAmount = side == SideOfSprite.Left ? -_distanceToMoveSprite : _distanceToMoveSprite;
        spriteObj.transform.position = new Vector2(spriteObj.transform.position.x + moveAmount, spriteObj.transform.position.y);
    }

    [Button(ButtonSizes.Large)]
    public void ToggleParallax()
    {
        if (_parallaxActive)
        {
            Timing.KillCoroutines(_coroutineHandler);
            _parallaxActive = false;
        }
        else
        {
            _coroutineHandler = Timing.RunCoroutine(MovingSpritesCoroutine());
            _parallaxActive = true;
        }
    }

    private void ForceParallaxOn()
    {
        if(!_parallaxActive)
        {
            ToggleParallax();
        }
    }
}

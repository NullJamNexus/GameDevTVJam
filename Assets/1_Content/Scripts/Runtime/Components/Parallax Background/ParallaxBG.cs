using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using System.Collections.Generic;
using MEC;
using NJN.Runtime.Managers;
using Unity.Cinemachine;
using NJN.Runtime.Managers.Level.Signals;

public class ParallaxBG : MonoBehaviour
{
    private enum SideOfSprite
    {
        Left,
        Right
    }

    [InfoBox("ALL SPRITES MUST BE SAME SIZE")]
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("The distance difference between player and sprite before it is teleported, usually works well with (spritewidth + 10)")]
    private int _distanceDifferential;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("How far the sprite needs to be teleported, usually works well with ( (spritewidth +10) * 2)")]
    private int _distanceToMoveSprite;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("Lower number makes effect stronger and faster, recommended to keep around 10")]
    private float _speedOffset = 10f;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("How frequently sprites are translated when player reaches outer bounds (in seconds)")]
    private float _updateFrequency = 0.7f;

    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;

    [SerializeField, ReadOnly] public float _autoScrollIncrement;

    private LevelManager _levelManager;
    private CinemachineCamera _followCam;
    private bool _parallaxActive;
    private CoroutineHandle _coroutineHandler;
    private List<GameObject> _layerBackChildren;
    private List<GameObject> _layerMiddleChildren;
    private List<GameObject> _layerFrontChildren;
    private bool _autoScroll = true;
    private GameObject _centreObj;
    private float _currentScrollPos;

    private SignalBus _signalBus;
    private CoroutineHandle _accelerationHandle;

    [Inject]
    private void Construct(LevelManager levelManager, [Inject(Id = "FollowCamera")] CinemachineCamera followCam, SignalBus signalBus)
    {
        _levelManager = levelManager;
        _followCam = followCam;
        _signalBus = signalBus;
    }

    private void Start()
    {
        InitializeLayers();
        Timing.RunCoroutine(InitializeCentreObject().CancelWith(this));
    }

    private IEnumerator<float> InitializeCentreObject()
    {
        while (_centreObj == null)
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
        _signalBus.Subscribe<DestinationTransitionStartedSignal>(OnDestinationTransitionStarted);
        _signalBus.Subscribe<DestinationTransitionFinishedSignal>(OnDestinationTransitionFinished);
    }

    private void OnDisable()
    {
        _signalBus.TryUnsubscribe<DestinationTransitionStartedSignal>(OnDestinationTransitionStarted);
        _signalBus.TryUnsubscribe<DestinationTransitionFinishedSignal>(OnDestinationTransitionFinished);
        Timing.KillCoroutines(_accelerationHandle);
    }

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
        for (int i = 0; i < layer.transform.childCount; i++)
        {
            children.Add(layer.transform.GetChild(i).gameObject);
        }

        if (children.Count != 3)
        {
            Debug.LogWarning($"PARALLAXBG: Layer {layer.name} has the wrong number of children!");
        }

        return children;
    }

    private void Update()
    {
        if (_parallaxActive)
        {
            if (_autoScroll)
            {
                if (_centreObj != null)
                {
                    ScrollLayersTransition(_currentScrollPos);
                    _currentScrollPos += _autoScrollIncrement;
                }
            }
            else
            {
                ScrollLayers();
            }
        }
    }

    private void ScrollLayers()
    {
        float centreObjPosX = _followCam.transform.position.x;
        _layerBack.transform.position = new Vector2(-centreObjPosX / _speedOffset, _layerBack.transform.position.y);
        _layerMiddle.transform.position = new Vector2(-centreObjPosX / (_speedOffset / 3), _layerMiddle.transform.position.y);
        _layerFront.transform.position = new Vector2(-centreObjPosX / (_speedOffset / 6), _layerFront.transform.position.y);
    }

    private void ScrollLayersTransition(float increment)
    {
        float centreObjPosX = _followCam.transform.position.x;
        _layerBack.transform.position = new Vector2(((-centreObjPosX + increment) / _speedOffset) % 40, _layerBack.transform.position.y);
        _layerMiddle.transform.position = new Vector2(((-centreObjPosX + increment) / (_speedOffset / 2)) % 40, _layerMiddle.transform.position.y);
        _layerFront.transform.position = new Vector2(((-centreObjPosX + increment) / (_speedOffset / 4)) % 40, _layerFront.transform.position.y);
    }
    
    private IEnumerator<float> MovingSpritesCoroutine()
    {
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
            float distanceToCentreObj = _autoScroll
                ? _centreObj != null
                    ? child.transform.position.x - _centreObj.transform.position.x
                    : 0
                : child.transform.position.x - _levelManager.Player.transform.position.x;

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
            _coroutineHandler = Timing.RunCoroutine(MovingSpritesCoroutine().CancelWith(this));
            _parallaxActive = true;
        }
    }

    private void ForceParallaxOn()
    {
        if (!_parallaxActive)
        {
            ToggleParallax();
        }
    }

    private void OnDestinationTransitionStarted(DestinationTransitionStartedSignal signal)
    {
        Timing.KillCoroutines(_accelerationHandle);
        _accelerationHandle = Timing.RunCoroutine(AccelerateToMaxSpeed(signal.MaxSpeed, signal.AccelerationRate).CancelWith(this));
    }

    private void OnDestinationTransitionFinished(DestinationTransitionFinishedSignal signal)
    {
        Timing.KillCoroutines(_accelerationHandle);
        _accelerationHandle = Timing.RunCoroutine(DecelerateToStop(signal.DecelerationRate).CancelWith(this));
    }

    private IEnumerator<float> AccelerateToMaxSpeed(float maxSpeed, float accelerationRate)
    {
        while (_autoScrollIncrement < maxSpeed)
        {
            _autoScrollIncrement += 0.1f;
            yield return Timing.WaitForSeconds(accelerationRate);
        }
        _autoScrollIncrement = maxSpeed;
    }

    private IEnumerator<float> DecelerateToStop(float decelerationRate)
    {
        while (_autoScrollIncrement > 0.1f)
        {
            _autoScrollIncrement -= 0.1f;
            yield return Timing.WaitForSeconds(decelerationRate);
        }
        _autoScrollIncrement = 0;
    }
}

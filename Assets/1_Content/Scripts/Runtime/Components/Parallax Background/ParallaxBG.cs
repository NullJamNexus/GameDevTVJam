using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;
using System.Collections.Generic;
using MEC;

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
    private float speedOffset = 10f;

    private LevelManager _levelManager;
    private bool _parallaxActive;
    private CoroutineHandle _coroutineHandler;
    private List<GameObject> _layerBackChildren;
    private List<GameObject> _layerMiddleChildren;
    private List<GameObject> _layerFrontChildren;

    // TODO: Change LevelManager to its interface
    [Inject]
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void Start()
    {
        InitializeLayers();
        ToggleParallax();
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
        if (_parallaxActive)
        {
            ScrollLayers();
        }
    }

    private void ScrollLayers()
    {
        //TODO: Does this need delta time because its being called in update?
        float playerPosX = _levelManager.Player.transform.position.x;
        _layerBack.transform.position = -(new Vector2(playerPosX, 0) / speedOffset);
        _layerMiddle.transform.position = -(new Vector2(playerPosX, 0) / (speedOffset / 2));
        _layerFront.transform.position = -(new Vector2(playerPosX, 0) / (speedOffset / 4));
    }

    private IEnumerator<float> MovingSpritesCoroutine()
    {
        yield return Timing.WaitForOneFrame;

        while (_parallaxActive)
        {
            AdjustSpritePositions(_layerBackChildren);
            AdjustSpritePositions(_layerMiddleChildren);
            AdjustSpritePositions(_layerFrontChildren);

            yield return Timing.WaitForSeconds(1f);
        }
    }

    private void AdjustSpritePositions(List<GameObject> layerChildren)
    {
        foreach (GameObject child in layerChildren)
        {
            float distanceToPlayer = child.transform.position.x - _levelManager.Player.transform.position.x;
            if (distanceToPlayer > _distanceDifferential)
            {
                MoveSprite(SideOfSprite.Left, child);
            }
            else if (distanceToPlayer < -_distanceDifferential)
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
            _coroutineHandler = Timing.RunCoroutine(MovingSpritesCoroutine().CancelWith(gameObject));
            _parallaxActive = true;
        }
    }
}

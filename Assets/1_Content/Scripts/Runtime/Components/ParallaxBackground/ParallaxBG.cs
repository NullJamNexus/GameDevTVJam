using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using NJN.Runtime.Managers;
using System.Collections.Generic;
using MEC;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField, ReadOnly] private GameObject _layerBack;
    [SerializeField, ReadOnly] private GameObject _layerMiddle;
    [SerializeField, ReadOnly] private GameObject _layerFront;

    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("The distance difference between player and sprite before it is teleported")]
    private int _distanceDifferential;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("How far the sprite needs to be teleported")]
    private int _distanceToMoveSprite;
    [SerializeField, BoxGroup("Tweak Values (Read Tooltips)"), Tooltip("Higher number = slower")]
    private float speedOffset;

    private LevelManager _levelManager;
    private bool _parallaxActive;
    private List<GameObject> _layerBackChildren = new ();

    [Inject]
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void Awake()
    {
        InitializeLayers();
        StartParallax();
    }

    private void Start()
    {
        Timing.RunCoroutine(CheckIfSpriteNeedsMovingCoroutine().CancelWith(gameObject));
    }

    private void Update()
    {
        if (_parallaxActive)
        {
            ScrollLayers();
        }
    }

    private void InitializeLayers()
    {
        _layerBack = transform.GetChild(0).gameObject;
        _layerMiddle = transform.GetChild(1).gameObject;
        _layerFront = transform.GetChild(2).gameObject;

        for (int i = 0; i < 3; i++)
        {
            if (_layerBack.transform.childCount >= 3)
            {
                _layerBackChildren.Add(_layerBack.transform.GetChild(i).gameObject);
            }
            else
            {
                Debug.LogWarning("PARALLAXBG: Layer has less than three children!");
                break;
            }
        }
    }

    private void ScrollLayers()
    {
        float playerPosX = _levelManager.Player.transform.position.x;
        _layerBack.transform.position = -(new Vector2(playerPosX, 0) / speedOffset);
        _layerMiddle.transform.position = -(new Vector2(playerPosX, 0) / (speedOffset / 2));
        _layerFront.transform.position = -(new Vector2(playerPosX, 0) / (speedOffset / 4));
    }

    private IEnumerator<float> CheckIfSpriteNeedsMovingCoroutine()
    {
        yield return Timing.WaitForOneFrame;

        while (_parallaxActive)
        {
            foreach (GameObject child in _layerBackChildren)
            {
                float distanceToPlayer = child.transform.position.x - _levelManager.Player.transform.position.x;
                if (Mathf.Abs(distanceToPlayer) > _distanceDifferential)
                {
                    float moveAmount = distanceToPlayer > 0 ? -_distanceToMoveSprite : _distanceToMoveSprite;
                    child.transform.position = new Vector2(child.transform.position.x + moveAmount, child.transform.position.y);
                }

                yield return Timing.WaitForSeconds(0.3f);
            }
        }
    }

    [Button(ButtonSizes.Large)]
    public void StartParallax()
    {
        if (_parallaxActive) return;
        
        _parallaxActive = true;
        Timing.RunCoroutine(CheckIfSpriteNeedsMovingCoroutine().CancelWith(gameObject));
    }

    [Button(ButtonSizes.Large)]
    public void StopParallax()
    {
        _parallaxActive = false;
    }
}

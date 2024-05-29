using UnityEngine;
using Sirenix.OdinInspector;
using MEC;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

using Unity.Cinemachine;
using NJN.Runtime.UI;




public class FadeToBlack : MonoBehaviour
{
    private Image _blackSquareSprite;
    private Color _spriteColor;
    private bool _isFading;
    private bool _isTransitioning;
    private CoroutineHandle _fadeToBlackCoroutineHandle;

    private CinemachineCamera _camera;
    private GameObject _transitionCamera;
    private CinemachineBrain _cmBrain;
    private PlayerHUD _hudObj;
    private GameObject _parallaxObj;


    [Inject]
    private void Construct([Inject(Id = "FollowCamera")] CinemachineCamera camera, [Inject(Id = "HUD")] PlayerHUD hudObj)
    {
        _camera = camera;  
        _hudObj = hudObj;
    }

    private void Start()
    {
        _blackSquareSprite = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
        _spriteColor = _blackSquareSprite.color;
        _transitionCamera = transform.GetChild(0).transform.GetChild(1).gameObject;
        _parallaxObj = transform.GetChild(1).gameObject;

        _transitionCamera.gameObject.SetActive(false);
        _parallaxObj.SetActive(false);

        ResetAlpha();
    }




    [Button(ButtonSizes.Gigantic)]
    private void TransitionToggle()
    {
        //Debug.Log("FADETOBLACK: is currently fading: "+_isFading);

        if(!_isFading)
        {
        Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
        _fadeToBlackCoroutineHandle = Timing.RunCoroutine(FadeToBlackCoroutine());
        }
        else
        {
            Debug.Log("FadeToBlack.cs: Unable to start transition as one is already in progress.");
        }

    }

    private void ResetAlpha(){

        _spriteColor.a = 0;
        _blackSquareSprite.color = _spriteColor;
    }

    private IEnumerator<float> FadeToBlackCoroutine()
    {
        //TODO:
        //Stop player input here
        _parallaxObj.SetActive(true);
        yield return Timing.WaitForSeconds(1);


        if(_spriteColor.a==0)
        {
            _isFading = true;
            while(_spriteColor.a!=1)
            {
                FadeToBlackProcess(true);
                yield return Timing.WaitForSeconds(0.05f);
            }
            
        }

        else if(_spriteColor.a==1)
        {
            _isFading = true;
            while(_spriteColor.a!=0)
            {
                FadeToBlackProcess(false);
                yield return Timing.WaitForSeconds(0.05f);
            }
            
        }

        //TODO:
        //Resume player input here
    
    }

    private void FadeToBlackProcess(bool direction)
    {

        float increment;

        if(direction) increment=0.1f;

        else increment=-0.1f;
        
        _spriteColor.a +=increment;

        _blackSquareSprite.color = _spriteColor;

        //Debug.Log($"FADETOBLACK: {_spriteColor.a}");

        if(_spriteColor.a>1)
        {
            _spriteColor.a = 1;
            _isFading = false;
            Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
            TransitionToggle();
            ChangeCamera();
        }
        else if(_spriteColor.a<0)
        {
            _spriteColor.a = 0;
            _isFading = false;
            Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
            
        }

    }

    private void ChangeCamera()
    {

        if(!_isTransitioning)
        {
        _hudObj.gameObject.SetActive(false);
        _camera.transform.gameObject.SetActive(false);
        _transitionCamera.gameObject.SetActive(true);
        _isTransitioning=true;
        }

        else
        {
        _hudObj.gameObject.SetActive(true);
        _camera.transform.gameObject.SetActive(true);
        _transitionCamera.gameObject.SetActive(false);
        _parallaxObj.SetActive(false);
        _isTransitioning=false;            
        }

    }
    

}

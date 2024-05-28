using UnityEngine;
using Sirenix.OdinInspector;
using MEC;
using System.Collections.Generic;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    private Image _blackSquareSprite;
    private Color _spriteColor;
    private bool _isFading;
    private CoroutineHandle _fadeToBlackCoroutineHandle;
    private void Start()
    {
        _blackSquareSprite = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        _spriteColor = _blackSquareSprite.color;
        ResetAlpha();
    }




    [Button(ButtonSizes.Gigantic)]
    private void FadeToBlackGo()
    {
        Debug.Log("FADETOBLACK: is currently fading: "+_isFading);

        if(!_isFading)
        {
        Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
        _fadeToBlackCoroutineHandle = Timing.RunCoroutine(FadeToBlackCoroutine());
        }

    }

    private void ResetAlpha(){

        _spriteColor.a = 0;
        _blackSquareSprite.color = _spriteColor;
    }

    private IEnumerator<float> FadeToBlackCoroutine()
    {



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

    
    }

    private void FadeToBlackProcess(bool direction)
    {

        float increment;

        if(direction) increment=0.1f;

        else increment=-0.1f;
        
        _spriteColor.a +=increment;

        _blackSquareSprite.color = _spriteColor;

        Debug.Log($"FADETOBLACK: {_spriteColor.a}");

        if(_spriteColor.a>1)
        {
            _spriteColor.a = 1;
            _isFading = false;
            Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
        }
        else if(_spriteColor.a<0)
        {
            _spriteColor.a = 0;
            _isFading = false;
            Timing.KillCoroutines(_fadeToBlackCoroutineHandle);
            
        }

    }
    

}

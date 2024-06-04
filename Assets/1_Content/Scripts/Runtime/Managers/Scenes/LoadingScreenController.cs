using System;
using System.Collections.Generic;
using MEC;
using NJN.Runtime.Managers.Scenes.Signals;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers.Scenes
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField]
        private float _fadeDuration = 1f;
        
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("[LoadingScreenController] Canvas not found.");
            }
            
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
            if (_canvasGroup == null)
            {
                Debug.LogError("[LoadingScreenController] CanvasGroup not found.");
            }
            
            _canvas.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LoadingFadeSignal>(OnFade);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<LoadingFadeSignal>(OnFade);
        }
        
        private void OnFade(LoadingFadeSignal signal)
        {
            if (signal.Direction == LoadingFadeSignal.FadeDirection.In)
            {
                Timing.RunCoroutine(FadeIn(_fadeDuration, signal.OnFadeComplete).CancelWith(this));
            }
            else
            {
                Timing.RunCoroutine(FadeOut(_fadeDuration, signal.OnFadeComplete).CancelWith(this));
            }
        }

        private IEnumerator<float> FadeIn(float duration, Action onFadeComplete)
        {
            _canvas.gameObject.SetActive(true);
            
            _canvasGroup.alpha = 0f;
            
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            
            _canvasGroup.alpha = 1f;
            onFadeComplete?.Invoke();
        }
        
        private IEnumerator<float> FadeOut(float duration, Action onFadeComplete)
        {
            _canvas.gameObject.SetActive(false);
            _canvas.gameObject.SetActive(true);
            
            _canvasGroup.alpha = 1f;
            
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            
            _canvasGroup.alpha = 0f;
            onFadeComplete?.Invoke();
            
            _canvas.gameObject.SetActive(false);
        }
    }
}

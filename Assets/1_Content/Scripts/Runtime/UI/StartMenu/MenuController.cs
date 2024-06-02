using NJN.Runtime.Components;
using NJN.Runtime.Managers.Signals;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

namespace NJN.Runtime.UI.StartMenu
{
    public class StartMenu : BaseComponent
    {
        [BoxGroup("Menu"), SerializeField]
        private GameObject _menuPanel;

        [BoxGroup("Play"), SerializeField]
        private Button _playButton;

        [BoxGroup("Settings"), SerializeField]
        private Button _settingsButton;
        [BoxGroup("Settings"), SerializeField]
        private GameObject _settingsPanel;

        [BoxGroup("Credits"), SerializeField]
        private Button _creditsButton;
        [BoxGroup("Credits"), SerializeField]
        private GameObject _creditsPanel;

        [BoxGroup("Back Buttons"), SerializeField]
        private Button[] _backButtons;
        
        [BoxGroup("Exit"), SerializeField]
        private Button _exitButton;
        
        [BoxGroup("Animation"), SerializeField]
        private float _duration = 1f;

        private SignalBus _signalBus;

        private Vector2 _menuPanelStartPosition;
        private RectTransform _menuPanelRectTransform;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _menuPanelRectTransform = _menuPanel.GetComponent<RectTransform>();
            StoreMenuPanelStartPosition();
            AnimateMenuPanel();
        }

        private void StoreMenuPanelStartPosition()
        {
            _menuPanelStartPosition = _menuPanelRectTransform.anchoredPosition;
        }

        private void AnimateMenuPanel()
        {
            float offScreenY = -Screen.height;
            _menuPanelRectTransform.anchoredPosition = new Vector2(_menuPanelStartPosition.x, offScreenY);

            _menuPanelRectTransform.DOAnchorPos(_menuPanelStartPosition, _duration).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                SetButtonsInteractable(true);
            });
        }

        private void SetButtonsInteractable(bool interactable)
        {
            _playButton.interactable = interactable;
            _settingsButton.interactable = interactable;
            _creditsButton.interactable = interactable;
            _exitButton.interactable = interactable;
            foreach (Button button in _backButtons)
            {
                button.interactable = interactable;
            }
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlay);
            _settingsButton.onClick.AddListener(OnSettings);
            _creditsButton.onClick.AddListener(OnCredits);
            _exitButton.onClick.AddListener(OnExit);
            foreach (Button button in _backButtons)
                button.onClick.AddListener(OnBackButton);
        }

        private void OnDisable()
        {
            _playButton.onClick?.RemoveListener(OnPlay);
            _settingsButton?.onClick?.RemoveListener(OnSettings);
            _creditsButton?.onClick?.RemoveListener(OnCredits);
            _exitButton?.onClick?.RemoveListener(OnExit);
            foreach (Button button in _backButtons)
                button.onClick.RemoveListener(OnBackButton);
        }

        private void OnPlay()
        {
            _signalBus.Fire(new PlayPressedSignal());
        }

        private void OnSettings()
        {
            _settingsPanel.SetActive(true);
            _menuPanel.SetActive(false);
        }

        private void OnCredits()
        {
            _creditsPanel.SetActive(true);
            _menuPanel.SetActive(false);
        }

        private void OnBackButton()
        {
            _settingsPanel.SetActive(false);
            _creditsPanel.SetActive(false);
            _menuPanel.SetActive(true);
        }
        
        private void OnExit()
        {
            Application.Quit();
        }
    }
}

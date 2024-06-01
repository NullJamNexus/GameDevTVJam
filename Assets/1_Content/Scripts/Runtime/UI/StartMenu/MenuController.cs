using NJN.Runtime.Components;
using NJN.Runtime.Managers.Scenes;
using NJN.Runtime.Managers.Signals;
using NJN.Scriptables;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;


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
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlay);
            _settingsButton.onClick.AddListener(OnSettings);
            _creditsButton.onClick.AddListener(OnCredits);
            foreach(Button button in _backButtons)
                button.onClick.AddListener(OnBackButton);
        }

        private void OnDisable()
        { 
            _playButton.onClick?.RemoveListener(OnPlay);
            _settingsButton?.onClick?.RemoveListener(OnSettings);
            _creditsButton?.onClick?.RemoveListener(OnCredits);
            foreach (Button button in _backButtons)
                button.onClick.RemoveListener(OnBackButton);
        }

        private void OnPlay()
        {
            _signalBus.Fire<PlayPressedSignal>();
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


    }
}

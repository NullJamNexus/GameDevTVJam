using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace NJN.Runtime.UI.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [BoxGroup("Buttons"), SerializeField] Button _pauseButton;
        [BoxGroup("Buttons"), SerializeField] Button _restartButton;
        [BoxGroup("Buttons"), SerializeField] Button _settingsButton;
        [BoxGroup("Buttons"), SerializeField] Button _settingsBackbutton;
        [BoxGroup("Buttons"), SerializeField] Button _mainMenuButton;

        [BoxGroup("Panels"), SerializeField] GameObject _BackGround;
        [BoxGroup("Panels"), SerializeField] GameObject _pauseMenu;
        [BoxGroup("Panels"), SerializeField] GameObject _settingsPanel;

        private void Start()
        {
            _pauseButton.onClick.AddListener(OnPause);
            _restartButton.onClick.AddListener(OnRestart);
            _settingsButton.onClick.AddListener(OnSettings);
            _settingsBackbutton.onClick.AddListener(OnSettingsBackMenu);
            _mainMenuButton.onClick.AddListener(OnMainMenu);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPause);
            _restartButton.onClick.RemoveListener(OnRestart);
            _settingsButton.onClick.RemoveListener(OnSettings);
            _settingsBackbutton.onClick.RemoveListener(OnSettingsBackMenu);
            _mainMenuButton.onClick.RemoveListener(OnMainMenu);
        }

        public void StartPause()
        {
            _BackGround.SetActive(true);
        }
        private void OnPause()
        {
            _BackGround.SetActive(false);
            // Vit
        }

        private void OnRestart()
        {
            //Vit
        }
        private void OnMainMenu()
        {
            //Vit
        }
        private void OnSettings()
        {
            _pauseMenu.SetActive(false);
            _settingsPanel.SetActive(true);
        }
        private void OnSettingsBackMenu()
        {
            _pauseMenu.SetActive(true);
            _settingsPanel.SetActive(false);
        }

        
    }
}

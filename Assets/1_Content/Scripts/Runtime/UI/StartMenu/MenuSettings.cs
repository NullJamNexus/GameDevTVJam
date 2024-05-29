using NJN.Runtime.Components;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;


namespace NJN.Runtime.UI.StartMenu
{
    public class MenuSettings : BaseComponent
    {
        [BoxGroup("Master"), SerializeField]
        private Slider _masterSlider;

        [BoxGroup("Sfx"), SerializeField]
        private Slider _sfxSlider;

        [BoxGroup("Music"), SerializeField]
        private Slider _musicSlider;

        private void Start()
        {
            _masterSlider.onValueChanged.AddListener(OnMasterChanged);
            _sfxSlider.onValueChanged.AddListener(OnSfxChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        private void OnDestroy()
        {
            _masterSlider?.onValueChanged.RemoveListener(OnMasterChanged);
            _sfxSlider?.onValueChanged?.RemoveListener(OnSfxChanged);
            _musicSlider?.onValueChanged.RemoveListener(OnMusicChanged) ;
        }

        private void OnMasterChanged(float value)
        {
            print(value);
        }
        private void OnSfxChanged(float value)
        {

        }
        private void OnMusicChanged(float value)
        {

        }
    }
}
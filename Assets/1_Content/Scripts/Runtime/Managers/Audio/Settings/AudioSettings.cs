using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace NJN.Runtime.Settings.Audio
{
    public class AudioSettings : MonoBehaviour
    {
        [FoldoutGroup("Sliders"), SerializeField]
        private Slider _masterSlider;
        [FoldoutGroup("Sliders"), SerializeField]
        private Slider _sfxSlider;
        [FoldoutGroup("Sliders"), SerializeField]
        private Slider _musicSlider;


        readonly private string _masterParameter = "Master_Volume";
        readonly private string _musicParameter = "Music_Volume";
        readonly private string _sfxParameter = "SFX_Volume";
        void Start()
        {
            _masterSlider.value = GetGlobalParamater(_masterParameter);
            _musicSlider.value = GetGlobalParamater(_musicParameter);
            _sfxSlider.value = GetGlobalParamater(_sfxParameter);

            _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
            _sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
            _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            
        }
        private void OnDestroy()
        {
            _masterSlider?.onValueChanged.RemoveAllListeners();
            _sfxSlider?.onValueChanged.RemoveAllListeners();
            _musicSlider?.onValueChanged.RemoveAllListeners();
        }
        private void ChangeMasterVolume(float value)
        {
            SetGlobalParamater(_masterParameter, value);
        }
        private void ChangeSfxVolume(float value)
        {
            SetGlobalParamater(_sfxParameter, value);
        }
        private void ChangeMusicVolume(float value)
        {
            SetGlobalParamater(_musicParameter, value);
        }

        private float GetGlobalParamater(string paramaterName)
        {
            RuntimeManager.StudioSystem.getParameterByName(paramaterName, out float value);
            return value;
        }

        private void SetGlobalParamater(string paramaterName, float value)
        {
            RuntimeManager.StudioSystem.setParameterByName(paramaterName,value);

        }
    }
}
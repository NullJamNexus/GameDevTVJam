using NJN.Runtime.Components;
using NJN.Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NJN.Runtime.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [FoldoutGroup("Stats"), SerializeField]
        private Image _healthBar;
        [FoldoutGroup("Stats"), SerializeField]
        private Image _hungerBar;
        [FoldoutGroup("Stats"), SerializeField]
        private Image _thirstBar;

        [FoldoutGroup("Progress"), SerializeField]
        private RectTransform _progressIndicator;
        
        [FoldoutGroup("Timer"), SerializeField]
        private TMP_Text _timerText;
        
        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _foodText;
        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _fuelText;
        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _scrapsText;
        
        private ISurvivalStats _survivalStats;
        private LevelInventory _levelInventory;
        
        public void SetUp(ISurvivalStats survivalStats, LevelInventory levelInventory)
        {
            _survivalStats = survivalStats;
            _levelInventory = levelInventory;
            BindProperties();
        }
        
        private void BindProperties()
        {
            _survivalStats.HungerStat.Current.Subscribe(value =>
            {
                _hungerBar.fillAmount = value / _survivalStats.HungerStat.Max.Value;
            }).AddTo(this);

            _survivalStats.ThirstStat.Current.Subscribe(value =>
            {
                _thirstBar.fillAmount = value / _survivalStats.ThirstStat.Max.Value;
            }).AddTo(this);

            _survivalStats.HealthStat.Current.Subscribe(value =>
            {
                _healthBar.fillAmount = value / _survivalStats.HealthStat.Max.Value;
            }).AddTo(this);
            
            _levelInventory.Food.Subscribe(value =>
            {
                _foodText.text = $"{value} Food";
            }).AddTo(this);
            
            _levelInventory.Fuel.Subscribe(value =>
            {
                _fuelText.text = $"{value} Fuel";
            }).AddTo(this);
            
            _levelInventory.Scraps.Subscribe(value =>
            {
                _scrapsText.text = $"{value} Scraps";
            }).AddTo(this);
        }
    }
}
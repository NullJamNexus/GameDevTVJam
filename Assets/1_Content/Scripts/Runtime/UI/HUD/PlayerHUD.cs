using System;
using NJN.Runtime.Components;
using NJN.Runtime.Managers;
using NJN.Runtime.Managers.Level.Signals;
using NJN.Runtime.UI.Panels;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        [FoldoutGroup("Progress"), SerializeField]
        private float _maxIndicatorPosition = -912.7f;
        [FoldoutGroup("Progress"), SerializeField]
        private Image _progressFill;
        
        [FoldoutGroup("Timer"), SerializeField]
        private TMP_Text _timerText;
        [FoldoutGroup("Timer"), SerializeField]
        private GameObject _timerPanel;

        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _foodText;
        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _fuelText;
        [FoldoutGroup("Resources"), SerializeField]
        private TMP_Text _scrapsText;
        
        private ISurvivalStats _survivalStats;
        private ILevelInventory _levelInventory;
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(ILevelInventory levelInventory, SignalBus signalBus)
        {
            _levelInventory = levelInventory;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<ProgressUpdatedSignal>(OnDestinationSelected);
            _signalBus.Subscribe<EnteredTruckSignal>(HideTimer);
            _signalBus.Subscribe<ExitedTruckSignal>(ShowTimer);
        }

        private void Start()
        {
            _progressFill.fillAmount = 0f;
            _progressIndicator.anchoredPosition = new Vector2(0f, _progressIndicator.anchoredPosition.y);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ProgressUpdatedSignal>(OnDestinationSelected);
            _signalBus.TryUnsubscribe<EnteredTruckSignal>(HideTimer);
            _signalBus.TryUnsubscribe<ExitedTruckSignal>(ShowTimer);
        }

        public void SetUp(ISurvivalStats survivalStats)
        {
            _survivalStats = survivalStats;
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
        
        private void OnDestinationSelected(ProgressUpdatedSignal signal)
        {
            UpdateProgress(signal.CurrentDestination, signal.TotalDestinations);
        }

        private void UpdateProgress(int currentLocation, int totalLocations)
        {
            if (totalLocations <= 0)
                return;

            float progress = (float)currentLocation / totalLocations;

            _progressFill.fillAmount = progress;

            float indicatorPosition = progress * _maxIndicatorPosition;
            _progressIndicator.anchoredPosition = new Vector2(indicatorPosition, _progressIndicator.anchoredPosition.y);
        }

        private void ShowTimer()
        {
            _timerPanel.SetActive(true);
        }

        private void HideTimer()
        {
            _timerPanel.SetActive(false);
        }
    }
}
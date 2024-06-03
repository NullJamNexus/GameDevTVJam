using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using NJN.Runtime.Factories;
using NJN.Runtime.Managers;
using NJN.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NJN.Runtime.UI.Panels
{
    public class DestinationPanel : BasePanel
    {
        [BoxGroup("General"), SerializeField]
        private Button _closeButton;
        
        [BoxGroup("Destination Settings"), SerializeField]
        private DestinationOptionVisual _optionVisualPrefab;
        [BoxGroup("Destination Settings"), SerializeField]
        private GameObject _optionsParent;
        [BoxGroup("Destination Settings"), SerializeField]
        private int _maxOptionsCount = 4;

        private List<DestinationOptionVisual> _destinationOptions = new();
        private bool _hasAllOptions;
        private IDestinationsFactory _destinationsFactory;
        private ILevelInventory _levelInventory;
        
        [Inject]
        private void Construct(IDestinationsFactory destinationsFactory, ILevelInventory levelInventory)
        {
            _destinationsFactory = destinationsFactory;
            _levelInventory = levelInventory;
        }

        public override void SetUpPanel(GameUI gameUI, SignalBus signalBus)
        {
            base.SetUpPanel(gameUI, signalBus);
                
            _closeButton.onClick.AddListener(() => gameUI.TogglePanel(this, false));
            
            _signalBus.Subscribe<PickDestinationSignal>(OnPickDestination);
            
            if (!_hasAllOptions)
                CreateNewDestinationOptions();
        }
        
        public override void DisposePanel()
        {
            base.DisposePanel();
            
            _signalBus.TryUnsubscribe<PickDestinationSignal>(OnPickDestination);
            
            _closeButton.onClick.RemoveAllListeners();
        }
        
        private void OnPickDestination(PickDestinationSignal signal)
        {
            if (!_hasAllOptions)
                CreateNewDestinationOptions();
            else
            {
                foreach (DestinationOptionVisual option in _destinationOptions)
                {
                    ValidateOption(option);
                }
            }
            
            _gameUI.TogglePanel(this, true);
        }

        private void CreateNewDestinationOptions()
        {
            foreach (DestinationOptionVisual option in _destinationOptions)
            {
                Destroy(option.gameObject);
            }
            
            _destinationOptions.Clear();
            
            for (int i = 0; i < _maxOptionsCount; i++)
            {
                DestinationOptionVisual optionVisual = _destinationsFactory.CreateRandomOption(_optionsParent, _optionVisualPrefab);
                optionVisual.SetUpCallback(OnOptionSelected);
                ValidateOption(optionVisual);
                _destinationOptions.Add(optionVisual);
            }
            
            _hasAllOptions = true;
        }
        
        private void ValidateOption(DestinationOptionVisual destinationOption)
        {
            int fuelCost = destinationOption.DestinationData.FuelCost;
            destinationOption.SetInteractable(fuelCost <= _levelInventory.Fuel.Value);
        }
        
        private void OnOptionSelected(DestinationOptionVisual destinationOption)
        {
            _gameUI.TogglePanel(this, false);
            _hasAllOptions = false;
            _destinationsFactory.RemoveDestinationOption(destinationOption.DestinationData);
            _signalBus.Fire(new DestinationSelectedSignal(destinationOption.DestinationData));
        }
    }
}
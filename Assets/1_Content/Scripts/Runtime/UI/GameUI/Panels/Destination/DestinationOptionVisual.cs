using System;
using NJN.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NJN.Runtime.UI.Panels
{
    public class DestinationOptionVisual : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _nameText;
        [SerializeField]
        private TextMeshProUGUI _costText;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;

        private Button _optionButton;
        private Action<DestinationOptionVisual> _onOptionSelected;
        
        public DestinationOptionSO DestinationData { get; private set; }

        private void Awake()
        {
            _optionButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _optionButton.onClick.AddListener(() => _onOptionSelected?.Invoke(this));
        }

        private void OnDisable()
        {
            _optionButton.onClick.RemoveAllListeners();
        }

        public void SetUpFromFactory(DestinationOptionSO destinationData)
        {
            DestinationData = destinationData;
            _nameText.text = destinationData.DestinationName;
            _costText.text = $"COST: {destinationData.FuelCost} Fuel";
            _descriptionText.text = destinationData.DestinationInformation;
        }
        
        public void SetInteractable(bool interactable)
        {
            _optionButton.interactable = interactable;
        }
        
        public void SetUpCallback(Action<DestinationOptionVisual> onOptionSelected)
        {
            _onOptionSelected = onOptionSelected;
        }
    }
}
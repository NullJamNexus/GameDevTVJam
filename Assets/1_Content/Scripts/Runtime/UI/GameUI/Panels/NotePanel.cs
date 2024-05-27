using System;
using NJN.Runtime.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NJN.Runtime.UI.Panels
{
    public class NotePanel : BasePanel
    {
        [SerializeField]
        private TextMeshProUGUI _headerText;
        [SerializeField]
        private TextMeshProUGUI _bodyText;
        [SerializeField]
        private Button _closeButton;
        
        public override void SetUpPanel(GameUI gameUI, SignalBus signalBus)
        {
            base.SetUpPanel(gameUI, signalBus);
            
            _signalBus.Subscribe<ReadNoteSignal>(OnShowReadbaleNote);
            
            _closeButton.onClick.AddListener(() => gameUI.TogglePanel(this, false));
        }

        public override void DisposePanel()
        {
            base.DisposePanel();
            
            _signalBus.TryUnsubscribe<ReadNoteSignal>(OnShowReadbaleNote);
            
            _closeButton.onClick.RemoveAllListeners();
        }
        
        private void OnShowReadbaleNote(ReadNoteSignal signal)
        {
            _headerText.text = signal.NoteHeader;
            _bodyText.text = signal.NoteBody;
            
            _gameUI.TogglePanel(this, true);
        }
    }
}
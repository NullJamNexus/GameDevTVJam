using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class ReadableNote : InteractableComponent
    {
        [BoxGroup("Note"), SerializeField]
        private string _noteHeader = "INSTRUCTIONS";
        [BoxGroup("Note"), SerializeField, TextArea]
        private string _noteBody = "- Something \n - Something \n - Something";
        
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            _signalBus.Fire(new ReadNoteSignal(_noteHeader, _noteBody));
        }
    }
}
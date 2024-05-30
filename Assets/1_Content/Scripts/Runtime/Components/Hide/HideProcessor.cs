using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Vit.Utilities;
using Zenject;

namespace NJN.Runtime.Components
{
    public class HideProcessor : BaseComponent, IHideProcessor
    {
        [field: BoxGroup("Hide Settings"), SerializeField]
        public float HiddenAlpha { get; private set; } = 0.3f;
        [BoxGroup("Hide Settings"), SerializeField, ValueDropdown(nameof(GetLayerNames))]
        private string _hiddenLayer;

        private string _startLayer;
        
        public string GetHiddenLayer()
        {
            return _hiddenLayer;
        }
        
        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }
    }
}
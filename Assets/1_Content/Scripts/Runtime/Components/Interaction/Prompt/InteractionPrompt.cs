using System;
using TMPro;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class InteractionPrompt : BaseComponent
    {
        [SerializeField]
        private TextMeshPro _promptText;

        private void Start()
        {
            HidePrompt();
        }

        public void ShowPrompt(string prompt, Vector2 position)
        {
            _promptText.text = prompt;
            gameObject.SetActive(true);
            transform.position = position;
        }
        
        public void HidePrompt()
        {
            gameObject.SetActive(false);
        }
    }
}
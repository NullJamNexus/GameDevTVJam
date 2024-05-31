using NJN.Runtime.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NJN.Runtime.UI.StartMenu
{
    public class MenuCredits : BaseComponent
    {
        [SerializeField] List<PersonDatar> persons;
        void Start()
        {
            foreach (PersonDatar person in persons)
            {
                person.button.onClick.AddListener(() => OnButtonPressed(person.url));
            }
        }


        private void OnButtonPressed(string url)
        {
            if (url != "")
                Application.OpenURL(url);
            else
                print("No Url Detected");
        }
    }

    [Serializable]
    struct PersonDatar
    {
        public Button button;
        public string url;
    }
}
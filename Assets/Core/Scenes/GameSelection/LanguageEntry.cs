using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class LanguageEntry : MonoBehaviour
    {
        public Toggle _languageToggle;
        public TextMeshProUGUI _languageTxt;

        //Event for Toggle
        //Update label
        public event Action OnLanguageEntryToggleEvent;

        public void Show(string language)
        {
            //Update label of Toggles
            //Add event for Toggle
            _languageToggle.onValueChanged.AddListener(delegate {
                OnToggleValueChanged();
            });

            _languageTxt.text = $"{language}";
        }

        private void OnToggleValueChanged()
        {
            OnLanguageEntryToggleEvent?.Invoke();
        }
    }
}

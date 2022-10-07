using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DomainEntry : MonoBehaviour
    {
        public Toggle _domainToggle;
        public TextMeshProUGUI _domainTxt;
        public string _domainCode;

        public event Action OnDomainEntryToggleEvent;

        public void Show(string code, string codeLabel)
        {
            _domainCode = code;
            _domainTxt.text = $"{codeLabel}";

            _domainToggle.onValueChanged.AddListener(delegate {
                OnDomainToggleValueChanged();
            });
        }

        private void OnDomainToggleValueChanged()
        {
            OnDomainEntryToggleEvent?.Invoke();
        }
    }
}

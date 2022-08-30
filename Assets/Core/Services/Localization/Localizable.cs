using Zenject;
using TMPro;
using UnityEngine;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Services.Localization
{
    public class Localizable : MonoBehaviour
    {
        [Inject] private LocalizationManager _localization;

        [field : SerializeField] public string LocalizeKey { get; private set; }
        [field : SerializeField] public TMP_Text LocalizedText { get; private set; }

        private void Start()
        {
            _localization.OnTranslateEvent += GetTranslation;
            GetTranslation();
        }

        private void GetTranslation()
        {
            //Debug.Log("[Localizable] Received event OnTranslateEvent !");
            LocalizedText.SetText(_localization.GetTranslation(LocalizeKey));
        }

        private void OnDestroy()
        {
            _localization.OnTranslateEvent -= GetTranslation;
        }
    }
}
using Zenject;
using TMPro;
using UnityEngine;

namespace StarterCore.Core.Services.Localization
{
    public class Localizable : MonoBehaviour
    {
        [Inject] private LocalizationManager _localization;

        [field: SerializeField] public string LocalizeKey { get; private set; }
        [field: SerializeField] public TMP_Text LocalizedText { get; private set; }

        private void OnEnable()
        {
            _localization.OnTranslateEvent += GetTranslation;
        }
        private void Start()
        {
            GetTranslation();
        }

        private void GetTranslation()
        {
            Debug.Log("Tranlating text...> " + _localization.GetTranslation(LocalizeKey)) ;
            LocalizedText.SetText(_localization.GetTranslation(LocalizeKey));
        }

        private void OnDestroy()
        {
            _localization.OnTranslateEvent -= GetTranslation;
        }
    }
}
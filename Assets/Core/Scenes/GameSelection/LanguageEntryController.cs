using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using TMPro;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class LanguageEntryController : MonoBehaviour
    {
        [SerializeField] private LanguageEntry _languageEntryTemplate;
        [SerializeField] private Transform _templateContainer;

        public event Action<List<string>> OnLanguageUpdateEvent;

        public List<string> SelectedLanguages;

        // Start is called before the first frame update
        public void Show(List<Scenario> scenarioList)
        {
            _languageEntryTemplate.gameObject.SetActive(false);

            List<string> languages = GetLanguages(scenarioList);

            foreach (string language in languages)
            {
                LanguageEntry instance = Instantiate(_languageEntryTemplate, _templateContainer);

                instance.gameObject.SetActive(true);
                instance.name = "Toggle" + language;
                instance.OnLanguageEntryToggleEvent += () => OnLanguageToggleCLicked(instance);
                instance.Show(language);
            }

            SelectedLanguages = languages;
        }

        private List<string> GetLanguages(List<Scenario> list)
        {
            List<String> languages = new List<string>();

            foreach(Scenario s in list)
            {
                if (!languages.Contains(s.LanguageTag)) languages.Add(s.LanguageTag );
            }
            return languages;
        }

        private void OnLanguageToggleCLicked(LanguageEntry toggle)
        {
            //Update states of toggles
            if (toggle._languageToggle.isOn)
            {
                if (!SelectedLanguages.Contains(toggle._languageTxt.text))
                {
                    SelectedLanguages.Add(toggle._languageTxt.text);
                }
            }
            else
            {
                SelectedLanguages.Remove(toggle._languageTxt.text);
            }

            OnLanguageUpdateEvent?.Invoke(SelectedLanguages);
        }

        private void DebugListLanguage()
        {
            foreach (string s in SelectedLanguages)
            {
                Debug.Log("Content of toggles" + s + " / ");
            }
        }
    }
}

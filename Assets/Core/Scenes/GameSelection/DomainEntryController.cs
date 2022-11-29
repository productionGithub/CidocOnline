using UnityEngine;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DomainEntryController : MonoBehaviour
    {
        [SerializeField] private DomainEntry _domainEntryTemplate;
        [SerializeField] private Transform _templateContainer;

        public event Action<List<string>> OnDomainUpdateEvent;

        private Dictionary<string, Dictionary<string, string>> _domainDictionary;
        public List<string> SelectedDomains;

        public void Show()
        {
            InitDomainDictionary();

            _domainEntryTemplate.gameObject.SetActive(false);

            foreach (string domainKey in _domainDictionary.Keys)
            {
                DomainEntry instance = Instantiate(_domainEntryTemplate, _templateContainer);

                instance.gameObject.SetActive(true);
                instance.name = "Toggle" + domainKey;
                instance.OnDomainEntryToggleEvent += () => OnDomainToggleCLicked(instance);
                instance.Show(domainKey, _domainDictionary[domainKey]["en"]);
            }

            //Init SelectedLanguages toggles with all languages
            InitFilteredDomains();
        }

        public void InitFilteredDomains()
        {
            foreach (string key in _domainDictionary.Keys)
            {
                SelectedDomains.Add(key.ToString());
            }
        }

        private void OnDomainToggleCLicked(DomainEntry toggle)
        {
            //Update states of toggles
            if (toggle._domainToggle.isOn)
            {
                if (!SelectedDomains.Contains(toggle._domainCode))
                {
                    SelectedDomains.Add(toggle._domainCode);
                }
                Debug.Log("After adding, List size is : " + SelectedDomains.Count);
            }
            else
            {
                //Debug.Log("Removing from list : " + toggle._domainTxt.text);
                SelectedDomains.Remove(toggle._domainCode);
                Debug.Log("After removing, List size is : " + SelectedDomains.Count);
            }

            //Fire event OnLanguageUpdate
            OnDomainUpdateEvent?.Invoke(SelectedDomains);
        }

        private void DebugListLanguage()
        {
            foreach (string s in SelectedDomains)
            {
                Debug.Log("Content of toggles" + s + " / ");
            }
        }

        private void InitDomainDictionary()
        {
            _domainDictionary = new Dictionary<string, Dictionary<string, string>>
            {
                { "HIS", new Dictionary<string, string> { {"en", "history" }, {"fr", "histoire"} } },
                { "ARC", new Dictionary<string, string> { {"en", "archeology" }, {"fr", "achéologie"} } },
                { "CUL", new Dictionary<string, string> { {"en", "culture" }, {"fr", "culture"} } },
                { "HUM", new Dictionary<string, string> { {"en", "Hum. Sci" }, {"fr", "Sci.Hum"} } }
            };
        }
    }
}

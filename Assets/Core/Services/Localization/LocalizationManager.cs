using UnityEngine;
using System.Collections.Generic;
using StarterCore.Core.Services.Navigation;
using Zenject;
using System;

using TMPro;

using StarterCore.Core.Utils;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.GameState;

using Newtonsoft.Json;
using StarterCore.Core.Services.Network.Models;


using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Services.Localization
{
    public class LocalizationManager : IInitializable
    {
        [Inject] private NavigationService _navService;
        [Inject] private MockNetService _netService;
        [Inject] private GameStateManager _gamestate;

        public event Action OnTranslateEvent;
        TranslationsModel _languageDictionary;

        public void Initialize()
        {
            _gamestate.OnLocaleChanged += () => SetLocaleDictionary();
            //Debug.Log("[Localization Manager] Initialized.");
            SetLocaleDictionary();
        }

        private async void SetLocaleDictionary()
        {
            //Debug.Log("[LocalizationManager : Set locale Dictionary");

            //Set the new locale from remote language files
            LocalesManifestModel manifestModel = await GetLocaleManifest();//Contains languages file paths

            //Debug.Log("MANIFEST MODEL IS " + manifestModel.Locales);

            //Test if game locale exists in manifestModel
            if (SearchLocaleMatch(manifestModel))
            {
                //Debug.Log("[Localization Manager] Locale found : " + _gamestate.Locale);
                //If yes, get the corresponding language file
                _languageDictionary = await GetLocaleDictionary(_gamestate.Locale);
            }
            else
            {
                Debug.Log("[Localization Manager] Locale not found in language file, falling back to default language : " + _gamestate.DefaultLocale);
                _gamestate.Locale = _gamestate.DefaultLocale;
                _languageDictionary = await GetLocaleDictionary(_gamestate.Locale);
            }

            OnTranslateEvent?.Invoke();//Refresh Localizables
        }

        public string GetTranslation(string key)
        {
            //Debug.Log("[LocalizationManager] GetTranslation() called with key " + key);
           
            string text=string.Empty;
            if(_languageDictionary != null)
            {
                text = _languageDictionary.StaticText[_navService.CurrentSceneName][key];
            }
            return text;
        }

        public async UniTask<LocalesManifestModel> GetLocaleManifest()
        {
            var result = await _netService.GetLocalesManifestFile();
            return result;
        }

        public async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            var result = await _netService.GetLocaleDictionary(locale);
            return result;
        }

        public bool SearchLocaleMatch(LocalesManifestModel m)
        {
            foreach (string l in m.Locales.Keys)
            {
                if (l.Equals(_gamestate.Locale))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
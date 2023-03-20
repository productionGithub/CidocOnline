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
using Mono.Collections.Generic;

namespace StarterCore.Core.Services.Localization
{
    public class LocalizationManager : IInitializable
    {
        [Inject] private NavigationService _navService;
        [Inject] private APIService _netService;
        [Inject] private GameStateManager _gamestate;

        public event Action OnTranslateEvent;
        TranslationsModel _languageDictionary;

        public void Initialize()
        {
            _gamestate.OnLocaleChanged += () => SetLocaleDictionary();
            SetLocaleDictionary();
        }

        private async void SetLocaleDictionary()
        {
            Debug.Log("[LocalizationMgr] English !");
            //Set the new locale from remote language files
            LocalesManifestModel manifestModel = await GetLocaleManifest();//Contains languages file paths

            //Test if game locale exists in manifestModel
            if (SearchLocaleMatch(manifestModel))
            {
                //If yes, get the corresponding language file
                _languageDictionary = await GetLocaleDictionary(_gamestate.Locale);
                Debug.Log("Locale changed to : " + _gamestate.Locale);
            }
            else
            {
                _gamestate.SetLocale(_gamestate.DefaultLocale);
                Debug.Log("Locale changed to : " + _gamestate.DefaultLocale);
                _languageDictionary = await GetLocaleDictionary(_gamestate.Locale);
            }

            OnTranslateEvent?.Invoke();//Refresh Localizables
        }

        public string GetTranslation(string key)
        {
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

        public void OnDestroy()
        {
            _gamestate.OnLocaleChanged -= () => SetLocaleDictionary();
        }
    }
}
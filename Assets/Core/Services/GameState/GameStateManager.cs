using Zenject;
using UnityEngine;
using System;

using UnityEngine.SceneManagement;

using StarterCore.Core.Services.Localization;
using StarterCore.Core.Services.Network.Models;

using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Services.GameState
{
    public class GameStateManager : IInitializable
    {
        //[Inject] private LocalizationController _localizationController;
        public event Action OnLocaleChanged;

        private GameStateModel gameState;
        private GameStateModel GameState { get { return gameState; } }

        public string Locale { get { return gameState.Locale; } set { GameState.Locale = value; } }
        public string DefaultLocale { get { return gameState.DefaultLocale; } }

        public void Initialize()
        {
            gameState = new GameStateModel
            {
                Locale = "pl",
                DefaultLocale = "en"
            };
            Debug.Log("[GameStateManager] Initialized.");
            //SetLocale(Locale);
        }

        public void SetLocale(string locale)
        {
            gameState.Locale = locale;
            OnLocaleChanged?.Invoke();
            Debug.Log("[GameStateManager] GameState set to language : " + Locale);
        }
    }
}



/*
 * 
 *         public async UniTask<List<InstanceCard>> LoadCards()
        {
            var cards = await InitInstancesDeck();
            Debug.Log("We update something... : " + cards[0].imageName);
            return cards;
        }

        private async UniTask<List<InstanceCard>> InitInstancesDeck()
        {
            jsonFilePath = Path.Combine(Application.streamingAssetsPath, jsonFileLocation);
            string fullPathName = Path.Combine(jsonFilePath, jsonFileName);

            var sr = new StreamReader(fullPathName);
            var fileContent = await sr.ReadToEndAsync();
            sr.Close();

            instanceCards = JsonConvert.DeserializeObject<List<InstanceCard>>(fileContent);
            return instanceCards;
        }

*/
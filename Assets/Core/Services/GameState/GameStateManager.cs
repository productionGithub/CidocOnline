using Zenject;
using UnityEngine;


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
        //Path to instances.json file
        //readonly private string jsonFileLocation = "web_main/DecksFiles/Instances/";
        //private string jsonFilePath;
        //private readonly string jsonFileName = "Instances.json";

        //Instance cards
        public List<InstanceCard> instanceCards;

        private GameStateModel gameState;
        public GameStateModel GameState { get { return gameState; } }

        public void SetLocale(string locale)
        {
            gameState.Lang = locale;
        }

        public void Initialize()
        {
            gameState = new GameStateModel
            {
                Lang = "en"
            };
            Debug.Log("GameState initialized with language : " + gameState.Lang);
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
using Zenject;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

//For conditional debug
public static class Trace
{
    [Conditional("TRACE_ON")]
    public static void Log(string msg)
    {
        Debug.Log(msg);
    }
}

namespace StarterCore.Core.Services.GameState
{
    public class GameStateManager : IInitializable
    {
        public event Action OnLocaleChanged;

        private GameStateModel gameStateModel;
        public GameStateModel GameStateModel { get { return gameStateModel; } }

        public string Locale { get { return gameStateModel.Locale; } set { gameStateModel.Locale = value; } }
        public string DefaultLocale { get { return gameStateModel.DefaultLocale; } }
        public string Username { get { return gameStateModel.Username; } set { gameStateModel.Username = value;  } }

        public void Initialize()
        {
            gameStateModel = new GameStateModel
            {
                Locale = "fr",
                DefaultLocale = "en",
                Username = string.Empty 
            };
            Trace.Log("[GameStateManager] Initialized.");
            SetLocale(Locale);
        }

        public void SetLocale(string locale)
        {
            gameStateModel.Locale = locale;
            OnLocaleChanged?.Invoke();
            Trace.Log("[GameStateManager] GameState set to language : " + Locale);
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
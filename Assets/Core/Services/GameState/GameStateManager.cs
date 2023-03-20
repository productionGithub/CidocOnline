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
        public int UserId { get { return gameStateModel.UserId; } set { gameStateModel.UserId = value; } }

        public void Initialize()
        {
            gameStateModel = new GameStateModel
            {
                Locale = "fr",
                DefaultLocale = "en",
                Username = string.Empty 
            };
            SetLocale(Locale);
        }

        public void SetLocale(string locale)
        {
            Debug.Log("[GameStateMgr] English !");
            gameStateModel.Locale = locale;
            OnLocaleChanged?.Invoke();
        }
    }
}
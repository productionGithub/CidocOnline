using UnityEngine;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Navigation;
using Zenject;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.MainMenu
{

    public class MainMenuController : MonoBehaviour
    {
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        public void Show()
        {
            Debug.Log("[MainMenuController] Show !");
            HistoryModelDown bundle = new HistoryModelDown("", "", "", "", "");
            _navService.GetMainBundle(out bundle);
            
            Debug.Log(string.Format("Progression of player {5} is {0} / {1} / {2} / {3} / {4}",
                bundle.HistoryId, bundle.ScenarioName, bundle.ChapterName, bundle.ChallengeId, bundle.Score, _gameState.Username));
        }
    }
}
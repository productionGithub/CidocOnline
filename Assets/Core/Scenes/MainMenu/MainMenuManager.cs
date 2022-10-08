using UnityEngine;
using Zenject;


namespace StarterCore.Core.Scenes.MainMenu
{
    public class MainMenuManager : IInitializable
    {
        [Inject] MainMenuController _mainMenuController;

        public void Initialize()
        {
            Debug.Log("[MainMenuManager] Initialized!");
            _mainMenuController.Show();
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterCore.Core.Services.Navigation;
using Zenject;


namespace StarterCore.Core.Services.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        [Inject] private NavigationService _navService;


        // called first
        void Start()
        {
            TranslateStaticText(_navService.CurrentSceneName);
        }

        private void TranslateStaticText(string obj)
        {
            Debug.Log("[LocalizationManager] Translate static text in scene : " + obj);
        }

        //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    Debug.Log("OnSceneLoaded: " + scene.name);
        //    Debug.Log(mode);
        //}

        // called when the game is terminated
        //void OnDisable()
        //{
        //    SceneManager.sceneLoaded -= OnSceneLoaded;
        //}
    }
}





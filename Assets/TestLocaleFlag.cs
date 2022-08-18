using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Navigation;
using UnityEngine.SceneManagement;

public class TestLocaleFlag :
    MonoBehaviour
{
    [Inject] private GameStateManager _gamestate;
    public string locale;

    // Start is called before the first frame update
    void Start()
    {


    }

    public void ReloadScene()
    {
        _gamestate.SetLocale(locale);
        Debug.Log("Locale is now : " + _gamestate.Locale);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace StarterCore.Core.Services.Navigation
{
    public class NavigationService : IInitializable
    {
        [Inject] private ZenjectSceneLoader _sceneLoader;
        [Inject] private NavigationSetup _navSetup;

        public string CurrentSceneName { get; private set; }
        public bool FromPop { get; private set; }
        public int StackSize => _mainScenes.Count;

        //public event Action OnSceneChangeEvent;
        public event Action<string> OnSceneChangeEvent;


        private object _mainBundle;
        private Stack<(string sceneName, object bundle)> _mainScenes;
        private Dictionary<string, object> _contextScenes;

        public void Initialize()
        {
            if (SceneManager.sceneCount == 1)
            {
                CurrentSceneName = SceneManager.GetActiveScene().name;
            }
            else
            {
                int countLoaded = SceneManager.sceneCount;
                for (int i = 0; i < countLoaded; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (!_navSetup.Contains(scene.name))
                    {
                        CurrentSceneName = scene.name;
                        break;
                    }
                }
            }

            _mainScenes = new Stack<(string sceneName, object bundle)>();
            _contextScenes = new Dictionary<string, object>();
            LoadSetupScenes();
            _mainScenes.Push((CurrentSceneName, null));
        }

        public void Push(string sceneName, object bundle = null)
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                if (CurrentSceneName != sceneName)
                {
                    string previous = CurrentSceneName;

                    FromPop = false;
                    CurrentSceneName = sceneName;
                    _mainBundle = bundle;
                    _mainScenes.Push((sceneName, bundle));

                    LoadMainSceneAsync(sceneName, previous).Forget();
                }
                else
                {
                    Debug.LogError("[NavigationService] The given scene is already the loaded scene !");
                }
            }
            else
            {
                Debug.LogError("[NavigationService] The given scene name is null or empty !");
            }
        }

        public void Pop()
        {
            if (_mainScenes.Count <= 1) // The stack must never be empty !
            {
                Debug.LogError("[NavigationService] Can't pop because the stack contains only one element and must not be empty !");
            }
            else
            {
                FromPop = true;
                _mainScenes.Pop();

                (string sceneName, object bundle) first = _mainScenes.FirstOrDefault();
                if (first != default)
                {
                    string previous = CurrentSceneName;
                    CurrentSceneName = first.sceneName;
                    _mainBundle = first.bundle;

                    LoadMainSceneAsync(first.sceneName, previous).Forget();
                }
            }
        }

        public void Clear(string remainingSceneName, object bundle = null)
        {
            _mainScenes.Clear();
            Push(remainingSceneName, bundle);
        }

        public void Replace(string sceneName, object bundle = null)
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                _mainScenes.Pop();
                Push(sceneName, bundle);
            }
            else
            {
                Debug.LogError("[NavigationService] The given scene name is null or empty !");
            }
        }

        public void RefreshCurrentScene()
        {
            SceneManager.LoadScene(CurrentSceneName);
        }

        public void SetMainBundle(object bundle)
        {
            if (bundle != null && _mainScenes.Count > 0)
            {
                _mainBundle = bundle;
                (string sceneName, object bundle) mainSceneEntry = _mainScenes.Pop();
                _mainScenes.Push((mainSceneEntry.sceneName, bundle));

                // TODO: test if
                //(string sceneName, object bundle) couple = _mainScenes.Peek();
                //couple.bundle = bundle;
                // would work
            }
        }

        public bool GetMainBundle<T>(out T bundle)
        {
            bundle = default;
            if (_mainBundle != null)
            {
                if (_mainBundle is T)
                {
                    bundle = (T)_mainBundle;
                    return true;
                }
            }
            return false;
        }

        public async UniTask<T> GetMainBundleAsync<T>()
        {
            await UniTask.WaitUntil(() => _mainBundle != null);

            if (_mainBundle is T)
            {
                return (T)_mainBundle;
            }
            return default;
        }

        public async UniTask AddContextSceneAsync(string sceneName, object bundle = null)
        {
            if (!_contextScenes.ContainsKey(sceneName))
            {
                _contextScenes.Add(sceneName, bundle);
                await _sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }

        public async UniTask RemoveContextSceneAsync(string sceneName)
        {
            if (_contextScenes.ContainsKey(sceneName))
            {
                _contextScenes.Remove(sceneName);
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        public bool GetContextBundle<T>(string sceneName, out T bundle)
        {
            bundle = default;
            if (_contextScenes.ContainsKey(sceneName))
            {
                object value = _contextScenes[sceneName];
                if (value != null && value is T)
                {
                    bundle = (T)value;
                    return true;
                }
            }
            return false;
        }

        public async UniTask<T> GetContextBundleAsync<T>(string sceneName)
        {
            if (_contextScenes.ContainsKey(sceneName))
            {
                await UniTask.WaitUntil(() => _contextScenes[sceneName] != null);

                object value = _contextScenes[sceneName];
                if (value != null && value is T)
                {
                    return (T)value;
                }
            }
            return default;
        }


        private void LoadSetupScenes()
        {
            foreach (string sceneName in _navSetup.SetupScenes)
            {
                if (!AlreadyPresent(sceneName))
                {
                    AddContextSceneAsync(sceneName).Forget();
                }
            }
        }

        private bool AlreadyPresent(string sceneName)
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        private async UniTask LoadMainSceneAsync(string nextSceneName, string previousSceneName)
        {
            await _sceneLoader.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            _ = SceneManager.UnloadSceneAsync(previousSceneName);
            OnSceneChangeEvent?.Invoke(SceneManager.GetActiveScene().name);
        }

        public override string ToString()
        {
            string result = string.Empty;
            string bundledFormat = "[ {0}* ]";
            string noBundleFormat = "[ {0} ]";

            foreach ((string sceneName, object bundle) entry in _mainScenes)
            {
                if (entry.bundle != null)
                {
                    result += string.Format(bundledFormat, entry.sceneName);
                }
                else
                {
                    result += string.Format(noBundleFormat, entry.sceneName);
                }
            }
            return result;
        }

        private List<string> GetActiveScenesNames()
        {
            List<string> sceneNames = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                sceneNames.Add(scene.name);
            }
            return sceneNames;
        }
    }
}

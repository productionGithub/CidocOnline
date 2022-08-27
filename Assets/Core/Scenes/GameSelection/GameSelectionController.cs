using UnityEngine;
using System;
using Zenject;

using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionController : MonoBehaviour
    {

        [SerializeField] private GameSelection _template;
        [SerializeField] private Transform _parent;

        [Inject] private DiContainer _container;

        public GameSelection _gameSelection;


        public event Action OnBackEvent;

        public void Show()
        {
            // Disable template that is instanciated
            _template.gameObject.SetActive(false);
            _gameSelection = Instantiate(_template, _parent);
            _container.InjectGameObject(_gameSelection.gameObject);

            _gameSelection.gameObject.SetActive(true);
            _gameSelection.Show();
            _gameSelection.OnBackClickedEvent += BackClickedEvent;
        }

        private void BackClickedEvent()
        {
            Debug.Log("BACK TO USSR !");
            OnBackEvent?.Invoke();
        }

        private void OnDestroy()
        {
            _gameSelection.OnBackClickedEvent -= BackClickedEvent;
        }
    }
}

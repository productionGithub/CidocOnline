using UnityEngine;
using System;
using Zenject;

using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionController : MonoBehaviour
    {

        //[SerializeField] private GameSelection _template;
        //[SerializeField] private Transform _parent;

        [Header("Static")]
        [SerializeField] private Button _BackButton;
        [SerializeField] private PanelController _panelController;
        [Inject] private DiContainer _diContainer;


        public event Action OnBackClickedEvent;
        public event Action<string> OnPanelClickedEvent;



        //PanelController _panel;


        public event Action OnBackEvent;

        public void Show(List<Panel> entries)
        {

            _panelController.Show(entries);



            //_panelTemplate.gameObject.SetActive(false);
            //foreach(PanelEntry entry in entries)
            //{
            //    PanelController panel = Instantiate(_panelTemplate, _templateContainer);

            //    panel.GetComponent<PanelController>().OnGamePanelClickEvent += GamePanelClicked;
            //    panel._gameName.text = entry.name;
            //    panel..text = entry.name;
            //    panel.gameObject.SetActive(true);

            //    _diContainer.InjectGameObject(_panelTemplate.gameObject);
            //    panel.Show();
            //}


            _BackButton.onClick.AddListener(BackClickedEvent);
            _panelController.OnGamePanelClickEvent += PanelClicked;

            //// Disable template that is instanciated
            //_template.gameObject.SetActive(false);
            //_gameSelection = Instantiate(_template, _parent);
            //_container.InjectGameObject(_gameSelection.gameObject);

            //_gameSelection.gameObject.SetActive(true);
            //_gameSelection.Show();
            //_gameSelection.OnBackClickedEvent += BackClickedEvent;
        }

        private void PanelClicked(string panelName)
        {
            OnPanelClickedEvent?.Invoke(panelName);
        }

        private void BackClickedEvent()
        {
            Debug.Log("BACK TO USSR !");
            OnBackEvent?.Invoke();
        }

        private void OnDestroy()
        {
            OnBackClickedEvent -= BackClickedEvent;
        }
    }
}

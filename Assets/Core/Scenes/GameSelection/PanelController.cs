using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelController : MonoBehaviour
    {

        [Header("Dynamic")]
        [SerializeField] private PanelEntry _panelTemplate;
        [SerializeField] private Transform _templateContainer;

        //[SerializeField] private TextMeshProUGUI _gameName;


        private List<PanelEntry> _entries;

        public event Action<string> OnGamePanelClickEvent;

        public void Show(List<Panel> panels)
        {
            Clear();//Clear panel list _entries


            /*
            foreach (User user in users)
            {
                FormEntry instance = Instantiate(_template, _container);
                instance.Show(user);
                instance.OnDeleteClickEvent += () => OnDelete(instance, user);
                instance.gameObject.SetActive(true);
                _entries.Add(instance);
            }
            */
            //_templateContainer.gameObject.SetActive(false);

            foreach (Panel panel in panels)
            {
                PanelEntry instance = Instantiate(_panelTemplate, _templateContainer);
                instance.Show(panel);
                instance.OnClickEvent += GamePanelClicked;
                //instance.GetComponent<PanelController>().OnGamePanelClickEvent += GamePanelClicked;
                instance.gameObject.SetActive(true);
                _entries.Add(instance);

                //_diContainer.InjectGameObject(_panelTemplate.gameObject);

            }
        }

        private void GamePanelClicked(string panelName)
        {
            OnGamePanelClickEvent?.Invoke(panelName);
            //Debug.Log("Panel clicked -> " + panelName);
        }

        private void Clear()
        {
            if (_entries == null)
            {
                _entries = new List<PanelEntry>();
                _panelTemplate.gameObject.SetActive(false); // Disable template
                //_refreshButton.onClick.AddListener(() => OnRefreshClickedEvent?.Invoke());
            }
            else
            {
                foreach (var entry in _entries)
                {
                    Destroy(entry.gameObject);
                }
                _entries.Clear();
            }
        }
    }
}

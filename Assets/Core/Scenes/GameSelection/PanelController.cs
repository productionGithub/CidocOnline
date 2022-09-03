using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using StarterCore.Core.Services.Network.Models;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelController : MonoBehaviour
    {

        [Header("Dynamic")]
        [SerializeField] private PanelEntryController _panelTemplate;
        [SerializeField] private Transform _templateContainer;

        private List<PanelEntryController> _entries;

        public event Action<string> OnGamePanelClickEvent;

        public void Show(List<Panel> panels)
        {
            Clear();//Clear panel list _entries

            foreach (Panel panel in panels)
            {
                PanelEntryController instance = Instantiate(_panelTemplate, _templateContainer);
                instance.Show(panel);
                instance.OnPanelClickEvent += GamePanelClicked;
                instance.gameObject.SetActive(true);
                _entries.Add(instance);
            }
        }

        //TODO Va sauter au profit de la vue detail...
        private void GamePanelClicked(string panelName)
        {
            OnGamePanelClickEvent?.Invoke(panelName);
            //Debug.Log("Panel clicked -> " + panelName);
        }

        private void Clear()
        {
            if (_entries == null)
            {
                _entries = new List<PanelEntryController>();
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

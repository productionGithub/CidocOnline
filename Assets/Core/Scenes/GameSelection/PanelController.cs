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

        public event Action<string, string> OnPanelControllerPlayChapterEvent;

        public void Show(List<Scenario> scenariiPanels)
        {

            Clear();//Clear panel list _entries

            foreach(Scenario scenario in scenariiPanels)
            {
                PanelEntryController instance = Instantiate(_panelTemplate, _templateContainer);
                instance.Show(scenario);
                instance.OnPanelEntryControllerPlayEvent += OnPlayChapterClicked;
                instance.gameObject.SetActive(true);
                _entries.Add(instance);
            }
        }

        private void OnPlayChapterClicked(string scenarioTitle, string chapterName)
        {
            OnPanelControllerPlayChapterEvent?.Invoke(scenarioTitle, chapterName);
        }

        private void Clear()
        {
            if (_entries == null)
            {
                _entries = new List<PanelEntryController>();
                _panelTemplate.gameObject.SetActive(false); // Disable template
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

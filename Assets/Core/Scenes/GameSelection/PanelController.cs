#define TRACE_ON
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
        public event Action<string, string> OnResetProgressionEvent_PanelCtrl;

        private List<PanelEntryController> _entriesList;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<PanelEntryController>();
            }
            else
            {
                foreach (PanelEntryController e in _entriesList)
                {
                    e.OnPanelEntryControllerPlayEvent -= OnPlayChapterClicked;
                    e.OnResetProgressionEvent_PanelEntryCtrl -= OnResetProgression;
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
        }

        //public void Show(List<Scenario> scenariiPanels, List<ChapterCompletionModelDown> c)
        public void Show(List<Scenario> scenariiPanels)
        {
            Clear();//Clear panel list _entries

            foreach(Scenario scenario in scenariiPanels)
            {
                //Await scenario progression ?
                PanelEntryController instance = Instantiate(_panelTemplate, _templateContainer);
                //instance.Init(scenario, c[i++]);
                instance.Init(scenario);
                instance.Show();
                instance.OnPanelEntryControllerPlayEvent += OnPlayChapterClicked;
                instance.OnResetProgressionEvent_PanelEntryCtrl += OnResetProgression;
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

        private void OnResetProgression(string chapterName, string scenarioname)
        {
            Debug.Log("RESET ! -> PanelController");
            OnResetProgressionEvent_PanelCtrl?.Invoke(chapterName, scenarioname);
        }

    }
}

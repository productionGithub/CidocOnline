#define TRACE_OFF
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Network.Models;


namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelController : MonoBehaviour
    {
        [Inject] DiContainer _diContainer;

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

        public void Show(List<Scenario> scenariiPanels, List<ChapterProgressionModelDown> userProgression)
        {
            Clear();//Clear panel list _entries

            foreach(Scenario scenario in scenariiPanels)
            {
                PanelEntryController instance = Instantiate(_panelTemplate, _templateContainer);
                _diContainer.InjectGameObject(instance.gameObject);

                instance.Init(scenario);
                instance.Show(userProgression);//scenariiPanels, 
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
            OnResetProgressionEvent_PanelCtrl?.Invoke(chapterName, scenarioname);
        }

    }
}

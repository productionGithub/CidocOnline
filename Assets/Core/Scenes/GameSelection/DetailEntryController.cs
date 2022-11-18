using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DetailEntryController : MonoBehaviour
    {
        [SerializeField] private DetailEntry _detailEntryTemplate;
        [SerializeField] private Transform _templateContainer;

        public event Action<string> OnDetailEntryControllerPlayEvent;
        public event Action<string> OnResetProgressionEvent_EntryCtrl;

        private List<DetailEntry> _entriesList;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<DetailEntry>();
            }
            else
            {
                foreach (DetailEntry e in _entriesList)
                {
                    e.OnDetailEntryPlayEvent -= OnPlayChapterClicked;
                    e.OnResetProgressionEvent_Entry -= OnResetProgression;
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
        }

        public void Show(List<Chapter> chapters)
        {
            _detailEntryTemplate.gameObject.SetActive(false);
            //int i = 0;
            foreach (Chapter chapter in chapters)
            {
                DetailEntry instance = Instantiate(_detailEntryTemplate, _templateContainer);
                instance.gameObject.SetActive(true);
                //instance.Show(chapter, chapterCompletions.Completions[i]);
                instance.Init();
                instance.Show(chapter);
                instance.OnDetailEntryPlayEvent += OnPlayChapterClicked;
                instance.OnResetProgressionEvent_Entry += OnResetProgression;
            }
        }

        private void OnPlayChapterClicked(string chapterName)
        {
            OnDetailEntryControllerPlayEvent?.Invoke(chapterName);
        }

        private void OnResetProgression(string chapterName)
        {
            Debug.Log("RESET ! -> EntryController");
            OnResetProgressionEvent_EntryCtrl?.Invoke(chapterName);
        }

        public void OnDestroy()
        {
            
        }
    }
}

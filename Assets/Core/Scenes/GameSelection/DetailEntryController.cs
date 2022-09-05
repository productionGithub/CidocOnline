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

        public void Show(List<Chapter> chapters)
        {
            _detailEntryTemplate.gameObject.SetActive(false);
            foreach (Chapter chapter in chapters)
            {
                DetailEntry instance = Instantiate(_detailEntryTemplate, _templateContainer);
                instance.gameObject.SetActive(true);
                instance.OnDetailEntryPlayEvent += OnPlayChapterClicked;
                instance.Show(chapter);
            }
        }

        private void OnPlayChapterClicked(string chapterName)
        {
            OnDetailEntryControllerPlayEvent?.Invoke(chapterName);
        }
    }
}

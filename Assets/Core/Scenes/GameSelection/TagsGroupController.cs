using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class TagsGroupController : MonoBehaviour
    {
        //[Inject] private DiContainer _injecter;
        //[Inject] private GameStateManager _gameState;

        [SerializeField] private Button _domainTagTemplate;
        [SerializeField] private Button _ontologyTagTemplate;
        [SerializeField] private Button _authorTagTemplate;

        [SerializeField] private Transform _tagsContainer;

        //private Dictionary<string, Dictionary<string, string>> _tagsDic; 

        public void Show(List<string> domains, List<string> ontologies, List<string> authors)
        {
            foreach (string domain in domains)
            {
                _domainTagTemplate.gameObject.SetActive(false);

                //Dictionary<string, string> hisDic = _tagsDic[domain];
                Button domainTag = Instantiate(_domainTagTemplate, _tagsContainer);
                //_injecter.InjectGameObject(domainTag.gameObject);

                domainTag.GetComponentInChildren<TextMeshProUGUI>().text = domain;// _tagsDic[domain][hisDic[_gameState.Locale]];
                domainTag.gameObject.SetActive(true);
            }
            foreach (string ontology in ontologies)
            {
                _ontologyTagTemplate.gameObject.SetActive(false);
                Button ontologyTag = Instantiate(_ontologyTagTemplate, _tagsContainer);
                ontologyTag.GetComponentInChildren<TextMeshProUGUI>().text = ontology;
                ontologyTag.gameObject.SetActive(true);
            }
            foreach (string author in authors)
            {
                _authorTagTemplate.gameObject.SetActive(false);
                Button authorTag = Instantiate(_authorTagTemplate, _tagsContainer);
                authorTag.GetComponentInChildren<TextMeshProUGUI>().text = author;
                authorTag.gameObject.SetActive(true);
            }
        }

        //private void InitTagsDictionary()
        //{
        //    _tagsDic = new Dictionary<string, Dictionary<string, string>>
        //    {
        //        { "HIS", new Dictionary<string, string> { {"en", "history" }, {"fr", "histoire"} } },
        //        { "ARC", new Dictionary<string, string> { {"en", "archeology" }, {"fr", "achéologie"} } },
        //        { "CUL", new Dictionary<string, string> { {"en", "culture" }, {"fr", "culture"} } },
        //        { "HUM", new Dictionary<string, string> { {"en", "Hum. Sci" }, {"fr", "Sci.Hum"} } }
        //    };
        //}
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class TagsGroupController : MonoBehaviour
    {
        [SerializeField] private Button _domainTagTemplate;
        [SerializeField] private Button _ontologyTagTemplate;
        [SerializeField] private Button _authorTagTemplate;

        [SerializeField] private Transform _tagsContainer;

        public void Show(List<string> domains, List<string> ontologies, List<string> authors)
        {
            //domains
            foreach(string domain in domains)
            {
                _domainTagTemplate.gameObject.SetActive(false);
                Button domainTag = Instantiate(_domainTagTemplate, _tagsContainer);
                domainTag.GetComponentInChildren<TextMeshProUGUI>().text = domain;
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
    }
}
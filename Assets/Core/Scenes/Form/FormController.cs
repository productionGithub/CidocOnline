using CidocOnline2022.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CidocOnline2022.Core.Scenes.Form
{
    public class FormController : MonoBehaviour
    {
        [Header("Static")]
        [SerializeField] private Button _refreshButton;

        [Header("Dynamic")]
        [SerializeField] private FormEntry _template;
        [SerializeField] private Transform _container;

        private List<FormEntry> _entries;

        public event Action OnRefreshClickedEvent;

        private void Start()
        {
            _template.gameObject.SetActive(false);
            _refreshButton.onClick.AddListener(() => OnRefreshClickedEvent?.Invoke());
        }

        public void Show(List<User> users)
        {
            Clear();

            foreach (User user in users)
            {
                FormEntry instance = Instantiate(_template, _container);
                instance.Show(user);
                instance.OnDeleteClickEvent += () => OnDelete(instance, user);
                instance.gameObject.SetActive(true);
                _entries.Add(instance);
            }
        }

        private void OnDelete(FormEntry entry, User user)
        {
            _entries.Remove(entry);
            Debug.Log("Deleting user : " + user.FirstName);
            Destroy(entry.gameObject);
        }
        
        private void Clear()
        {
            if (_entries == null)
            {
                _entries = new List<FormEntry>();
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

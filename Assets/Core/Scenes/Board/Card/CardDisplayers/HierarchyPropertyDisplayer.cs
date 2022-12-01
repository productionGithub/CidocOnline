using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StarterCore.Core.Scenes.Board.Card.Cards;
using Cysharp.Threading.Tasks;


/// <summary>
/// Manager for PropertyCard Hierarchy scrollable view
/// </summary>
///

namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class HierarchyPropertyDisplayer : MonoBehaviour
    {

        public event Action<string> HierarchyPropertyEntryClickEvent;

        [SerializeField] HierarchyPropertyEntry _entryPropertyTemplate;
        [SerializeField] HierarchyCurrentEntry _currentEntryPropertyTemplate;

        public Transform _hierarchyPropertyContainer;//Parent object of scrollView

        private List<HierarchyPropertyEntry> _entriesProperty;//Keeps track of hierarchy instanciated prefabs
        private List<HierarchyCurrentEntry> _currentPropertyEntry;//Keeps track of hierarchy instanciated prefabs

        public void Init()
        {
            if (_entriesProperty == null)
            {
                _entriesProperty = new List<HierarchyPropertyEntry>();
            }
            else
            {
                foreach (HierarchyPropertyEntry e in _entriesProperty)
                {
                    e.OnHierarchyPropertyClickEvent -= HierarchyEntityPropertyClicked;
                    Destroy(e.gameObject);
                }
                _entriesProperty.Clear();
            }

            if (_currentPropertyEntry == null)
            {
                _currentPropertyEntry = new List<HierarchyCurrentEntry>();
            }
            else
            {
                foreach (HierarchyCurrentEntry e in _currentPropertyEntry)
                {
                    Destroy(e.gameObject);
                }
                _currentPropertyEntry.Clear();
            }
        }

        public void Show(PropertyCard card)
        {
            string arrowUp = "\u02C4";
            string arrowDown = "\u02C5";

            //Parents
            if (card.superProperties.Count > 0)
            {
                foreach (string label in card.superProperties)
                {
                    HierarchyPropertyEntry entry = Instantiate(_entryPropertyTemplate, _hierarchyPropertyContainer);
                    //entry.EntryLabel.text = arrowUp + label;
                    entry.Show(arrowUp, label);
                    entry.OnHierarchyPropertyClickEvent += HierarchyEntityPropertyClicked;

                    _entriesProperty.Add(entry);
                }
            }

            //Current card
            HierarchyCurrentEntry currentEntry = Instantiate(_currentEntryPropertyTemplate, _hierarchyPropertyContainer);
            currentEntry.Show(card.about);
            _currentPropertyEntry.Add(currentEntry);

            //Children
            if (card.subProperties.Count > 0)
            {
                foreach (string label in card.subProperties)
                {
                    HierarchyPropertyEntry entry = Instantiate(_entryPropertyTemplate, _hierarchyPropertyContainer);
                    //entry.EntryLabel.text = arrowUp + label;
                    entry.Show(arrowDown, label);
                    entry.OnHierarchyPropertyClickEvent += HierarchyEntityPropertyClicked;

                    _entriesProperty.Add(entry);
                }
            }

        }

        private void HierarchyEntityPropertyClicked(string label)
        {
            string id = label.Substring(0, label.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            HierarchyPropertyEntryClickEvent?.Invoke(id);
        }
    }
}

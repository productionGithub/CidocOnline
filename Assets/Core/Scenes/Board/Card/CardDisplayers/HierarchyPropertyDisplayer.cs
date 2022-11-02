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
/// Instantiate super and sub classes of a card (Buttons)
/// </summary>
namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class HierarchyPropertyDisplayer : MonoBehaviour
    {

        public event Action<string> HierarchyPropertyEntryClickEvent;

        [SerializeField]
        HierarchyPropertyEntry _entryPropertyTemplate;
        [SerializeField]
        HierarchyCurrentEntry _currentEntryPropertyTemplate;

        public Transform _hierarchyPropertyContainer;//Parent object of scrollView

        private List<HierarchyPropertyEntry> _entriesProperty;//Keeps track of hierarchy instanciated prefabs
        private List<HierarchyCurrentEntry> _currentPropertyEntry;//Keeps track of hierarchy instanciated prefabs

        //public void Show(EntityCard card)// List<EntityCard> parents, string current, List<EntityCard> children)
        //{
        //    CleanEntityHierarchy();
        //    DisplayEntityHierarchy(card);
        //}

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

        //public void CleanEntityHierarchy()
        //{
        //    //if (_entriesEntity == null)
        //    //{
        //    //    _entriesEntity = new List<HierarchyEntityEntry>();
        //    //}
        //    //else
        //    //{
        //    //    foreach (HierarchyEntityEntry e in _entriesEntity)
        //    //    {
        //    //        e.OnHierarchyEntityClickEvent -= HierarchyEntityEntryClicked;
        //    //        Destroy(e.gameObject);
        //    //    }
        //    //    _entriesEntity.Clear();
        //    //}

        //    //if (_currentEntityEntry == null)
        //    //{
        //    //    _currentEntityEntry = new List<HierarchyCurrentEntry>();
        //    //}
        //    //else
        //    //{
        //    //    foreach (HierarchyCurrentEntry e in _currentEntityEntry)
        //    //    {
        //    //        Destroy(e.gameObject);
        //    //    }
        //    //    _currentEntityEntry.Clear();
        //    //}
        //}



































        /*
        public event Action<string> HierarchyPropDisp_HierarchyClickEvent;

        [SerializeField]
        HierarchyPropertyEntry _entryTemplate;
        [SerializeField]
        HierarchyCurrentEntry _currentEntryTemplate;

        public Transform _hierarchyContainer;//Parent object of scrollView

        private List<HierarchyPropertyEntry> _entries;//Keeps track of hierarchy instanciated prefabs
        private List<HierarchyCurrentEntry> _currentEntry;//Keeps track of hierarchy instanciated prefabs

        public void Show(PropertyCard card)
        {
            Trace.Log("[HierarchyDisplayer] SHOW...");
            CleanHierarchy();
            DisplayHierarchy(card);
        }

        private void DisplayHierarchy(PropertyCard card)
        {
            string arrowUp = "\u02C4";
            string arrowDown = "\u02C5";

            //Parents
            if (card.superProperties.Count > 0)
            {
                foreach (string label in card.superProperties)
                {
                    HierarchyPropertyEntry entry = Instantiate(_entryTemplate, _hierarchyContainer);
                    //entry.EntryLabel.text = arrowUp + label;
                    entry.Show(arrowUp, label);
                    entry.OnHierarchyPropertyClickEvent += HierarchyEntryClicked;

                    _entries.Add(entry);
                    Trace.Log("");
                }
            }

            //Current card
            HierarchyCurrentEntry currentEntry = Instantiate(_currentEntryTemplate, _hierarchyContainer);
            currentEntry.Show(card.about);
            _currentEntry.Add(currentEntry);

            //Children
            if (card.subProperties.Count > 0)
            {
                foreach (string label in card.subProperties)
                {
                    HierarchyPropertyEntry entry = Instantiate(_entryTemplate, _hierarchyContainer);
                    //entry.EntryLabel.text = arrowUp + label;
                    entry.Show(arrowDown, label);
                    entry.OnHierarchyPropertyClickEvent += HierarchyEntryClicked;

                    _entries.Add(entry);
                }
            }

        }

        private void HierarchyEntryClicked(string label)
        {
            string id = label.Substring(0, label.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            HierarchyPropDisp_HierarchyClickEvent?.Invoke(id);
        }

        public void CleanHierarchy()
        {
            if (_entries == null)
            {
                Trace.Log("[HierarchyDisplayer] Entries list is null, create new one.");
                _entries = new List<HierarchyPropertyEntry>();
            }
            else
            {
                Trace.Log("[HierarchyDisplayer] Entries list not null, cleaning it.");
                foreach (HierarchyPropertyEntry e in _entries)
                {
                    Trace.Log("[HierarchyDisplayer] Cleaning : found object : " + e.GetComponent<HierarchyPropertyEntry>().EntryLabel.text);
                    e.OnHierarchyPropertyClickEvent -= HierarchyEntryClicked;
                    Destroy(e.gameObject);
                }
                _entries.Clear();
            }

            if (_currentEntry == null)
            {
                Trace.Log("[HierarchyCURRENTDisplayer] Entries list is null, create new one.");
                _currentEntry = new List<HierarchyCurrentEntry>();
            }
            else
            {
                Trace.Log("[HierarchyCURRENTDisplayer] Entries list not null, cleaning it.");
                foreach (HierarchyCurrentEntry e in _currentEntry)
                {
                    Trace.Log("[HierarchyDisplayer] Cleaning : found object : " + e.GetComponent<HierarchyCurrentEntry>().EntryLabel.text);
                    Destroy(e.gameObject);
                }
                _currentEntry.Clear();
            }
        }
        */
    }
}

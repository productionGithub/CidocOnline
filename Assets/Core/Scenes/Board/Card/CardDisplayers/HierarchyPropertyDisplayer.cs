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

public class HierarchyPropertyDisplayer : MonoBehaviour
{
    public event Action<string> HierarchyPropDisp_HierarchyClickEvent;

    [SerializeField]
    HierarchyEntityEntry _entryTemplate;
    [SerializeField]
    HierarchyCurrentEntry _currentEntryTemplate;

    public Transform _hierarchyContainer;//Parent object of scrollView

    private List<HierarchyEntityEntry> _entries;//Keeps track of hierarchy instanciated prefabs
    private List<HierarchyCurrentEntry> _currentEntry;//Keeps track of hierarchy instanciated prefabs

    public void Show(PropertyCard card)// List<EntityCard> parents, string current, List<EntityCard> children)
    {
        Debug.Log("[HierarchyDisplayer] SHOW...");
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
                HierarchyEntityEntry entry = Instantiate(_entryTemplate, _hierarchyContainer);
                //entry.EntryLabel.text = arrowUp + label;
                entry.Show(arrowUp, label);
                entry.OnHierarchyEntityClickEvent += HierarchyEntryClicked;

                _entries.Add(entry);
                Debug.Log("");
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
                HierarchyEntityEntry entry = Instantiate(_entryTemplate, _hierarchyContainer);
                //entry.EntryLabel.text = arrowUp + label;
                entry.Show(arrowDown, label);
                entry.OnHierarchyEntityClickEvent += HierarchyEntryClicked;

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
            Debug.Log("[HierarchyDisplayer] Entries list is null, create new one.");
            _entries = new List<HierarchyEntityEntry>();
        }
        else
        {
            Debug.Log("[HierarchyDisplayer] Entries list not null, cleaning it.");
            foreach (HierarchyEntityEntry e in _entries)
            {
                Debug.Log("[HierarchyDisplayer] Cleaning : found object : " + e.GetComponent<HierarchyEntityEntry>().EntryLabel.text);
                e.OnHierarchyEntityClickEvent -= HierarchyEntryClicked;
                Destroy(e.gameObject);
            }
            _entries.Clear();
        }

        if (_currentEntry == null)
        {
            Debug.Log("[HierarchyCURRENTDisplayer] Entries list is null, create new one.");
            _currentEntry = new List<HierarchyCurrentEntry>();
        }
        else
        {
            Debug.Log("[HierarchyCURRENTDisplayer] Entries list not null, cleaning it.");
            foreach (HierarchyCurrentEntry e in _currentEntry)
            {
                Debug.Log("[HierarchyDisplayer] Cleaning : found object : " + e.GetComponent<HierarchyCurrentEntry>().EntryLabel.text);
                Destroy(e.gameObject);
            }
            _currentEntry.Clear();
        }
    }
}

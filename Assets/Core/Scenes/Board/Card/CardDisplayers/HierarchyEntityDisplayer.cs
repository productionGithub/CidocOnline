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

public class HierarchyEntityDisplayer : MonoBehaviour
{
    public event Action<string> HierarchyEntDisp_HierarchyClickEvent;

    [SerializeField]
    HierarchyEntityEntry _entryEntityTemplate;
    [SerializeField]
    HierarchyCurrentEntry _currentEntryEntityTemplate;

    public Transform _hierarchyEntityContainer;//Parent object of scrollView

    private List<HierarchyEntityEntry> _entriesEntity;//Keeps track of hierarchy instanciated prefabs
    private List<HierarchyCurrentEntry> _currentEntityEntry;//Keeps track of hierarchy instanciated prefabs

    public void Show(EntityCard card)// List<EntityCard> parents, string current, List<EntityCard> children)
    {
        CleanEntityHierarchy();
        DisplayEntityHierarchy(card);
    }

    private void DisplayEntityHierarchy(EntityCard card)
    {
        string arrowUp = "\u02C4";
        string arrowDown = "\u02C5";

        //Parents
        if (card.parentsClassList.Count > 0)
        {
            foreach (string label in card.parentsClassList)
            {
                HierarchyEntityEntry entry = Instantiate(_entryEntityTemplate, _hierarchyEntityContainer);
                //entry.EntryLabel.text = arrowUp + label;
                entry.Show(arrowUp, label);
                entry.OnHierarchyEntityClickEvent += HierarchyEntityEntryClicked;

                _entriesEntity.Add(entry);
            }
        }

        //Current card
        HierarchyCurrentEntry currentEntry = Instantiate(_currentEntryEntityTemplate, _hierarchyEntityContainer);
        currentEntry.Show(card.about);
        _currentEntityEntry.Add(currentEntry);

        //Children
        if (card.ChildrenClassList.Count > 0)
        {
            foreach (string label in card.ChildrenClassList)
            {
                HierarchyEntityEntry entry = Instantiate(_entryEntityTemplate, _hierarchyEntityContainer);
                //entry.EntryLabel.text = arrowUp + label;
                entry.Show(arrowDown, label);
                entry.OnHierarchyEntityClickEvent += HierarchyEntityEntryClicked;

                _entriesEntity.Add(entry);
            }
        }

    }

    private void HierarchyEntityEntryClicked(string label)
    {
        string id = label.Substring(0, label.IndexOf("_")).Trim();//Fetch the sub string before first '_'
        HierarchyEntDisp_HierarchyClickEvent?.Invoke(id);
    }

    public void CleanEntityHierarchy()
    {
        if (_entriesEntity == null)
        {
            _entriesEntity = new List<HierarchyEntityEntry>();
        }
        else
        {
            foreach (HierarchyEntityEntry e in _entriesEntity)
            {
                e.OnHierarchyEntityClickEvent -= HierarchyEntityEntryClicked;
                Destroy(e.gameObject);
            }
            _entriesEntity.Clear();
        }

        if (_currentEntityEntry == null)
        {
            _currentEntityEntry = new List<HierarchyCurrentEntry>();
        }
        else
        {
            foreach (HierarchyCurrentEntry e in _currentEntityEntry)
            {
                Destroy(e.gameObject);
            }
            _currentEntityEntry.Clear();
        }
    }
}

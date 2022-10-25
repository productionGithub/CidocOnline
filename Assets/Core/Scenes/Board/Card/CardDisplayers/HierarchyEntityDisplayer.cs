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
    HierarchyEntityEntry _entryTemplate;
    [SerializeField]
    HierarchyCurrentEntry _currentEntry;

    public Transform _hierarchyContainer;//Parent object of scrollView

    private List<HierarchyEntityEntry> entries;//Keeps track of hierarchy instanciated prefabs

    public void Show(EntityCard card)// List<EntityCard> parents, string current, List<EntityCard> children)
    {
        Debug.Log("[HierarchyDisplayer] SHOW...");
        CleanHierarchy();
        DisplayHierarchy(card);
    }

    private void DisplayHierarchy(EntityCard card)
    {
        string arrowUp = "\u02C4";

        //Parents
        if (card.parentsClassList.Count > 0)
        {
            foreach (string label in card.parentsClassList)
            {
                HierarchyEntityEntry entry = Instantiate(_entryTemplate, _hierarchyContainer);
                //entry.EntryLabel.text = arrowUp + label;
                entry.Show(arrowUp, label);
                entry.OnHierarchyEntityClickEvent += HierarchyEntryClicked;

                entries.Add(entry);
                Debug.Log("");
            }
        }
    }

    private void HierarchyEntryClicked(string label)
    {
        string id = label.Substring(0, label.IndexOf("_")).Trim();//Fetch the sub string before first '_'
        HierarchyEntDisp_HierarchyClickEvent?.Invoke(id);
    }














    /*
    public void Show(EntityCard card)// List<EntityCard> parents, string current, List<EntityCard> children)
    {
        CleanHierarchy();

        string arrowUp = "\u02C4";
        string arrowDown = "\u02C5";

        //Parent classes
        if (card.parentsClassList.Count > 0)
        {
            foreach (string label in card.parentsClassList)
            {
                HierarchyEntityDisplayer parentEntry = Instantiate(_entryTemplate);
                var compo = parentEntry.GetComponent<HierarchyEntityEntry>();
                compo.Show(arrowUp, label);
                compo.OnHierarchyEntityClickEvent += AddEntry;
                parentEntry.transform.SetParent(_hierarchyContainer);

                entries.Add(compo);
            }
        }

        //Current
        HierarchyCurrentEntry currentButton = Instantiate(_currentEntry);
        currentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = card.about;
        currentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        currentButton.transform.SetParent(_hierarchyContainer);

        //Children
        if (card.ChildrenClassList.Count > 0)
        {
            foreach(string label in card.ChildrenClassList)
            {
                HierarchyEntityDisplayer childrenEntry = Instantiate(_entryTemplate);
                var compo = childrenEntry.GetComponent<HierarchyEntityEntry>();
                compo.Show(arrowDown,label);
                compo.OnHierarchyEntityClickEvent += AddEntry;
                childrenEntry.transform.SetParent(_hierarchyContainer);

                entries.Add(compo);
            }
        }
    }
    */

    public void CleanHierarchy()
    {
        if (entries == null)
        {
            Debug.Log("[HierarchyDisplayer] Entries list is null, create new one.");
            entries = new List<HierarchyEntityEntry>();
        }
        else
        {
            Debug.Log("[HierarchyDisplayer] Entries list not null, cleaning it.");
            foreach (HierarchyEntityEntry e in entries)
            {
                Debug.Log("[HierarchyDisplayer] Cleaning : found object : " + e.GetComponent<HierarchyEntityEntry>().EntryLabel.text);
                e.OnHierarchyEntityClickEvent -= HierarchyEntryClicked;
                Destroy(e.gameObject);
            }
            entries.Clear();
        }

    }
}


/*
static class Helper
{
    public static string GetUntilOrEmpty(this string text, string stopAt = "-")
    {
        if (!String.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return String.Empty;
    }
}
*/
